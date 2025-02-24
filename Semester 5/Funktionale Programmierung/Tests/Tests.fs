namespace Tests

open System.IO
open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open WarAndPeace.Program
open WarAndPeace.RBTree

[<AutoOpen>]
module TestHelpers =

    // timeAction:
    // Runs a given function and measures how long it takes.
    // Returns Ok(result, ms) if successful, allowing us to compose performance checks functionally.
    // This demonstrates how we can integrate performance measurement into a monadic workflow (Result).
    let timeAction action arg =
        let sw = System.Diagnostics.Stopwatch()
        sw.Start()
        let result = action arg
        sw.Stop()
        // Wrap result and elapsed time in `Ok` to integrate with the Result monad for clean error handling.
        Ok (result, sw.ElapsedMilliseconds)

    // rootIsBlack:
    // Checks if the RBTree's root is black or if the tree is empty.
    // This is part of the Red-Black Tree invariant checks.
    let rootIsBlack tree =
        match tree with
        | Empty -> true
        | Node(c,_,_,_) -> c = Black

    // noRedParentHasRedChild:
    // Ensures no red node has a red child—another RB invariant.
    // If a red parent had a red child, it would violate RBTree properties.
    let rec noRedParentHasRedChild t =
        match t with
        | Empty -> true
        | Node(c,l,_,r) ->
            let currentValid =
                match c with
                | Black -> true
                | Red ->
                    // Check left and right child colors
                    let leftValid =
                        match l with
                        | Empty -> true
                        | Node(Black,_,_,_) -> true
                        | Node(Red,_,_,_) -> false

                    let rightValid =
                        match r with
                        | Empty -> true
                        | Node(Black,_,_,_) -> true
                        | Node(Red,_,_,_) -> false

                    leftValid && rightValid
            currentValid && noRedParentHasRedChild l && noRedParentHasRedChild r

    // blackHeightPaths:
    // Gathers the "black-height" of every path from root to leaf.
    // Black-height = number of black nodes on that path.
    // Uses List.collect(higher-order function) instead of manual concatenation.
    let blackHeightPaths t =
        let rec paths tree acc =
            match tree with
            | Empty -> 
                // Reached a leaf: return the current black count as a single path result.
                [acc]
            | Node(c,l,_,r) ->
                let inc = if c = Black then acc + 1 else acc
                // Collect paths from left and right subtrees.
                [l; r]
                |> List.collect (fun subtree -> paths subtree inc)
        paths t 0

    // blackHeightInvariant:
    // Checks if all root-to-leaf paths have the same black-height.
    let blackHeightInvariant tree =
        let heights = blackHeightPaths tree
        (List.distinct heights).Length = 1

    // allInvariantsHold:
    // Combined check for RB invariants:
    //  - Root is black.
    //  - No red parent has a red child.
    //  - All paths have the same black-height.
    let allInvariantsHold tree =
        rootIsBlack tree &&
        noRedParentHasRedChild tree &&
        blackHeightInvariant tree

    // isSorted:
    // Checks if a given list of strings is sorted in ascending order.
    let isSorted (list: string list) =
        list
        |> Seq.pairwise
        |> Seq.forall (fun (a,b) -> a <= b)

    // isUnique:
    // Checks if all elements in the list are distinct.
    let isUnique (list: string list) =
        (List.distinct list).Length = list.Length

// WordGen:
// A custom FsCheck generator that ensures generated strings are alphabetic and lowercase.
// This integrates with our property tests, providing controlled inputs.
type WordGen() =
    static member Word() =
        Arb.Default.String()
        |> Arb.filter (fun s -> s <> null && s |> Seq.forall System.Char.IsLetter)
        |> Arb.convert _.ToLowerInvariant() id

[<SetUpFixture>]
type GlobalSetup() =
    [<OneTimeSetUp>]
    member _.RegisterArb() =
        // Register the custom WordGen so FsCheck uses it for strings in property tests.
        Arb.register<WordGen>() |> ignore

//----------------------------------------------
// Invariant Tests (Fixed Sets)
//----------------------------------------------
// Tests that insert a known, fixed set of words and verify RB invariants hold.
[<TestFixture; Category("Invariants")>]
type InvariantTests() =
    [<Test>]
    member _.``Fixed set of words - all RB invariants hold``() =
        // Insert a known set of words and check invariants.
        let words = ["cat"; "dog"; "apple"; "zebra"; "lion"; "lion"; "dog"]
        let tree = List.fold (fun acc w -> insert w acc) Empty words
        Assert.That(allInvariantsHold tree, Is.True, "Expected all RB invariants to hold.")

    [<Test>]
    member _.``Empty tree invariants``() =
        // Even an empty tree should trivially meet all RB invariants.
        Assert.That(allInvariantsHold Empty, Is.True, "Empty tree should satisfy invariants.")

//----------------------------------------------
// Property-Based Tests for Invariants
//----------------------------------------------
// Uses FsCheck to generate random word lists and ensures RB invariants still hold.
[<TestFixture; Category("Property")>]
type PropertyInvariantTests() =
    [<Property>]
    member _.``Random words still respect invariants``(words: string list) =
        let tree = List.fold (fun acc w -> insert w acc) Empty words
        allInvariantsHold tree

//----------------------------------------------
// Property-Based Tests for Sorting and Uniqueness
//----------------------------------------------
// Tests that after insertion and conversion to a list, the output is sorted and contains unique words.
[<TestFixture; Category("Property")>]
type PropertySortingTests() =
    [<Property>]
    member _.``Inserted words -> sorted & unique output``(words: string list) =
        let tree = List.fold (fun acc w -> insert w acc) Empty words
        let sortedList = toSortedList tree
        (isSorted sortedList) && (isUnique sortedList)

//----------------------------------------------
// End-to-End Test
//----------------------------------------------
// Reads "war_and_peace.txt", processes it, and checks that the output file contains words.
[<TestFixture; Category("EndToEnd")>]
type EndToEndTests() =
    [<Test>]
    member _.``Full run - War and Peace -> output file``() =
        let solutionDir = Directory.GetParent(__SOURCE_DIRECTORY__).FullName
        let inputPath = Path.Combine(solutionDir, "war_and_peace.txt")
        let outputPath = Path.Combine(solutionDir, "output.txt")

        // Call "processFile" function directly:
        let result = processFile inputPath outputPath

        match result with
        | Ok count ->
            Assert.That(count > 0, Is.True, "Expected some unique words in War and Peace.")
            Assert.That(File.Exists(outputPath), Is.True, "Expected 'output.txt' to be created.")
        | Error msg ->
            Assert.Fail($"Expected success but got error: {msg}")

//----------------------------------------------
// Performance / Proof-of-Concept Test
//----------------------------------------------
// Times the program's execution on large input and ensures it completes successfully.
[<TestFixture; Category("Performance")>]
type PerformanceTests() =
    [<Test>]
    member _.``Measure performance with large input``() =
        let result =
            timeAction main [||]
            |> Result.bind (fun (exitCode, ms) ->
                if exitCode = 0 then Ok ms else Error "Non-zero exit code"
            )

        match result with
        | Ok ms ->
            printfn $"Processing took %d{ms} ms"
            Assert.Pass()
        | Error msg ->
            Assert.Fail(msg)
