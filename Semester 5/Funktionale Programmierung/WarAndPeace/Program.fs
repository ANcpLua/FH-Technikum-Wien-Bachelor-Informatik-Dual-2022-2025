namespace WarAndPeace

open System
open System.IO
open System.Text.RegularExpressions

module RBTree =

    /// <summary>
    /// Possible colors of a node in the Red-Black Tree.
    /// </summary>
    type Color = 
        | Red
        | Black

    /// <summary>
    /// Red-Black Tree node definition.
    /// Each node contains a Color, left subtree, a string value, and a right subtree.
    /// Empty denotes an empty leaf node.
    /// </summary>
    type RBTree =
        | Empty
        | Node of Color * RBTree * string * RBTree

    /// <summary>
    /// Balances the Red-Black Tree after insertion. 
    /// If the tree violates the Red-Black properties (e.g., two red nodes in a row),
    /// this function performs a rotation or color flip to restore balance.
    /// </summary>
    /// <param name="c">Color of the current root node.</param>
    /// <param name="l">Left subtree.</param>
    /// <param name="v">String value of the root node.</param>
    /// <param name="r">Right subtree.</param>
    /// <returns>A balanced Red-Black Tree node.</returns>
    let private balance c l v r =
        match c, l, v, r with
        // Pattern 1: Left child is red, and its left child is also red.
        | Black, Node(Red, Node(Red,a,x,b), y, c1), z, d ->
            Node(Red, Node(Black,a,x,b), y, Node(Black,c1,z,d))
        // Pattern 2: Left child is red, and its right child is also red.
        | Black, Node(Red,a,x,Node(Red,b,y,c1)), z, d ->
            Node(Red, Node(Black,a,x,b), y, Node(Black,c1,z,d))
        // Pattern 3: Right child is red, and its left child is also red.
        | Black, a, x, Node(Red, Node(Red,b,y,c1), z, d) ->
            Node(Red, Node(Black,a,x,b), y, Node(Black,c1,z,d))
        // Pattern 4: Right child is red, and its right child is also red.
        | Black, a, x, Node(Red,b,y,Node(Red,c1,z,d)) ->
            Node(Red, Node(Black,a,x,b), y, Node(Black,c1,z,d))
        // No balancing needed.
        | _ -> Node(c,l,v,r)

    /// <summary>
    /// Inserts a new word into the Red-Black Tree.
    /// If the word already exists, the tree remains unchanged.
    /// After insertion, the root node is recolored black to maintain Red-Black properties.
    /// </summary>
    /// <param name="word">The string value (word) to insert.</param>
    /// <param name="tree">The current Red-Black Tree.</param>
    /// <returns>A new Red-Black Tree containing the inserted word.</returns>
    let insert word tree =
        // Inner recursive function that descends the tree and attempts insertion.
        let rec ins t =
            match t with
            | Empty ->
                // Found an empty leaf: create a new red node here.
                Node(Red, Empty, word, Empty)
            | Node(c,l,v,r) ->
                let cmp = compare word v
                if cmp = 0 then 
                    // Word already exists, do nothing.
                    Node(c,l,v,r)
                elif cmp < 0 then
                    // Insert into left subtree, then balance.
                    balance c (ins l) v r
                else
                    // Insert into right subtree, then balance.
                    balance c l v (ins r)

        // Insert the word using the inner function, then recolor the root to black.
        match ins tree with
        | Empty -> Empty
        | Node(_,l,v,r) -> Node(Black,l,v,r)

    /// <summary>
    /// Performs an in-order traversal of the Red-Black Tree to produce a sorted list of its words.
    /// </summary>
    /// <param name="t">The Red-Black Tree to traverse.</param>
    /// <returns>A sorted list of all string values in ascending order.</returns>
    let toSortedList t =
        let rec inorder tree =
            seq {
                match tree with
                | Empty ->
                    // No values in an empty node.
                    ()
                | Node(_, l, v, r) ->
                    // Traverse left subtree, yield node value, then traverse right subtree.
                    yield! inorder l
                    yield v
                    yield! inorder r
            }
        // Convert the lazy sequence to a concrete list.
        inorder t |> Seq.toList

module Text =

    /// <summary>
    /// Converts a string to lowercase and removes any non-letter characters.
    /// </summary>
    /// <param name="word">The original string (token) to normalize.</param>
    /// <returns>The normalized string containing only lowercase letters.</returns>
    let normalize (word: string) =
        word.ToLowerInvariant() |> String.filter Char.IsLetter

    /// <summary>
    /// Splits a text string into a list of clean words using regex, 
    /// then normalizes each word and filters out empty entries.
    /// </summary>
    /// <param name="text">The raw text to tokenize.</param>
    /// <returns>A list of lowercase words containing only letters.</returns>
    let tokenize (text: string) =
        let pattern = @"\b[a-zA-Z]+\b"
        Regex.Matches(text, pattern)
        |> Seq.cast<Match>
        |> Seq.map (fun m -> normalize m.Value)
        |> Seq.filter (fun w -> w.Length > 0)
        |> Seq.toList

module FileIO =

    /// <summary>
    /// Reads the entire content of a file, returning an Ok string or Error message.
    /// </summary>
    /// <param name="path">The path to the file to read.</param>
    /// <returns>A Result containing the file content or an error message.</returns>
    let readFile path =
        try
            File.ReadAllText(path) |> Ok
        with ex ->
            Error $"Failed to read file '{path}': {ex.Message}"

    /// <summary>
    /// Writes a list of lines to a file, returning an Ok unit or Error message.
    /// </summary>
    /// <param name="path">The path to the file to write.</param>
    /// <param name="lines">A list of lines to write to the file.</param>
    /// <returns>A Result indicating success or an error message.</returns>
    let writeFile path (lines: string list) =
        try
            File.WriteAllLines(path, lines)
            Ok()
        with ex ->
            Error $"Failed to write file '{path}': {ex.Message}"

module Program =
    open RBTree
    open Text
    open FileIO

    /// <summary>
    /// Processes a file by:
    /// 1. Reading the text.
    /// 2. Tokenizing to extract words.
    /// 3. Inserting all words into a Red-Black Tree.
    /// 4. Converting the tree to a sorted list of unique words.
    /// 5. Writing the sorted words to an output file.
    /// Returns the total count of unique words or an error message.
    /// </summary>
    /// <param name="inputPath">Path to the input text file.</param>
    /// <param name="outputPath">Path to the output file.</param>
    /// <returns>A Result representing the count of unique words or an error.</returns>
    let processFile inputPath outputPath =
        readFile inputPath
        |> Result.map tokenize
        |> Result.map (List.fold (fun acc w -> insert w acc) Empty)
        |> Result.map toSortedList
        |> Result.bind (fun sortedWords ->
            writeFile outputPath sortedWords
            |> Result.map (fun () -> sortedWords.Length)
        )

    /// <summary>
    /// The application's entry point. Determines input and output paths,
    /// then executes the file-processing pipeline and prints the result.
    /// </summary>
    /// <param name="argv">Command-line arguments for overriding the default paths.</param>
    /// <returns>An integer exit code: 0 indicates success; non-zero indicates an error.</returns>
    [<EntryPoint>]
    let main argv =
        // Determine the solution directory to locate the default input file.
        let solutionDir = Directory.GetParent(__SOURCE_DIRECTORY__).FullName
        let defaultPath = Path.Combine(solutionDir, "war_and_peace.txt")

        // If an argument is given, use it as input else use default "war_and_peace.txt".
        let inputPath =
            if argv.Length > 0 then argv[0]
            else defaultPath

        // Output file is always named "output.txt".
        let outputPath = Path.Combine(solutionDir, "output.txt")

        // Create the pipeline to read, process, and write.
        let result = processFile inputPath outputPath

        // Handle the result: print the number of unique words on success, show error on failure.
        match result with
        | Ok count ->
            printfn $"Processed {count} unique words."
            0
        | Error msg ->
            eprintfn $"Error: {msg}"
            1
