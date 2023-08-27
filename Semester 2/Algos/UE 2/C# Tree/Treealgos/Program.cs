


class TreeNode
{
    public int Key;
    public TreeNode? Left;
    public TreeNode? Right;
}

class BinaryTree
{
    public TreeNode? Root;

    public void Insert(int key)
    {
        Root = Insert(Root, key);
    }

    private static TreeNode? Insert(TreeNode? node, int key)
    {
        if (node == null)
        {
            return new TreeNode { Key = key };
        }

        if (key < node.Key)
        {
            node.Left = Insert(node.Left, key);
        }
        else if (key > node.Key)
        {
            node.Right = Insert(node.Right, key);
        }

        return node;
    }


    private int Height(TreeNode? node)
    {
        if (node == null)
        {
            return 0;
        }

        int leftHeight = Height(node.Left);
        int rightHeight = Height(node.Right);

        return Math.Max(leftHeight, rightHeight) + 1;
    }

    private int BalanceFactor(TreeNode? node)
    {
        if (node == null)
        {
            return 0;
        }

        return Height(node.Right) - Height(node.Left);
    }
    
    public void TraverseAndCheckAvl(TreeNode? node, ref bool isAVl)
    {
        if (node == null)
        {
            return;
        }
        
        
        TraverseAndCheckAvl(node.Right, ref isAVl);
        TraverseAndCheckAvl(node.Left, ref isAVl);

        int balance = BalanceFactor(node);
        Console.Write($"bal({node.Key}) = {balance}");
        if (balance > 1 || balance < -1)
        {
            Console.Write(" (AVL violation!)");
            isAVl = false;
        }
        Console.WriteLine();
        

    }

    public List<int> TraverseAndCollectKeys(TreeNode? node)
    {
        if (node == null)
        {
            return new List<int>();
        }

        return TraverseAndCollectKeys(node.Left)
            .Concat(new[] { node.Key })
            .Concat(TraverseAndCollectKeys(node.Right))
            .ToList();
    }



    public static bool PrintPathToKey(TreeNode? node, int key, List<int>? path = null)
    {
        if (node == null)
        {
            return false;
        }

        path ??= new List<int>();

        path.Add(node.Key);

        if (node.Key == key)
        {
            Console.WriteLine($"{key} found {string.Join(", ", path)}");
            return true;
        }

        bool foundInLeft = false;
        bool foundInRight = false;
        if (key < node.Key)
        {
            foundInLeft = PrintPathToKey(node.Left, key, path);
        }
        else
        {
            foundInRight = PrintPathToKey(node.Right, key, path);
        }

        path.RemoveAt(path.Count - 1);

        return foundInLeft || foundInRight;
    }

    
    public bool CheckSubtreeStructure(TreeNode? node, List<int> subtreeStructure)
    {
        if (subtreeStructure.Count == 0) 
        {
            return true;
        }
        if (node == null) 
        {
            return false;
        }

        int index = subtreeStructure.IndexOf(node.Key);
        if (index == 0)
        {
            subtreeStructure.RemoveAt(0);
        }

        return CheckSubtreeStructure(node.Left, subtreeStructure) ||
               CheckSubtreeStructure(node.Right, subtreeStructure);
    }

    
}


static class Program
{
    static void Main(string[] args)
    {
        
        if (args.Length != 3)
        {
            Console.WriteLine(" Max 3 files einlesen änder die 3 oder lösch die Zeile ODER Du hast die arguments vergessen: Falls du Raider benutzt dann geh oben rechts auf edit config und bei Programmm Übergabewerte : schreib das hier rein filename.txt filename-suchbaum.txt filename-subtree.txt in diese txt files schreibst du dann die angabe rein, und speicherst die .txt files im RiderProjects 2 - TREECHECK 2 - TREECHECK bin Debug net7.0   HIER ");
            return;
        }
        // Process the first file 
        ProcessFile1(args[0]);

        // Process the second and third file
        ProcessFiles(args[1], args[2]);
        ProcessFiles2(args[1], args[2]);
    }

    static void ProcessFile1(string filename)
    {
        Console.WriteLine($"Processing file: {filename}");

        var tree = new BinaryTree();

        using (StreamReader reader = new StreamReader(filename))
        {
            HashSet<int> addedKeys = new HashSet<int>();
            while (reader.ReadLine() is { } line)
            {
                int key = int.Parse(line);
                if (!addedKeys.Contains(key))
                {
                    tree.Insert(key);
                    addedKeys.Add(key);
                }
            }
        }
        
        bool isAvL= true;
        tree.TraverseAndCheckAvl(tree.Root, ref isAvL);
        Console.WriteLine("AVL: " + (isAvL ? "yes" : "no"));
        var keys = tree.TraverseAndCollectKeys(tree.Root);
        Console.WriteLine($"min: {keys.Min()}, max: {keys.Max()}, avg: {keys.Average():F1}");
        
    }
    
    static void ProcessFiles(string subtreeFile, string suchbaumFile )
    {
        Console.WriteLine($"Processing subtree file: {subtreeFile}");
        Console.WriteLine($"Processing suchbaum file: {suchbaumFile}");
        

        var mainTree = new BinaryTree();

        // Read and build the suchbaum
        using (var reader = new StreamReader(suchbaumFile))
        {
            while (reader.ReadLine() is { } line)
            {
                var key = int.Parse(line);
                mainTree.Insert(key);
            }
        }

        // Read the search key from the subtree file
        int searchKey;
        using (var reader = new StreamReader(subtreeFile))
        {
            var line = reader.ReadLine();
            if (line == null)
            {
                Console.WriteLine("No search key provided in the subtree file!");
                return;
            }
            searchKey = int.Parse(line);
        }

        bool found = BinaryTree.PrintPathToKey(mainTree.Root, searchKey);
        if (!found)
        {
            Console.WriteLine($"{searchKey} not found");
        }
        
        // die auskommentierte methode ließt 1 aus noch aus, einfach mit ProcessFiles austauschen
        
       /* static void ProcessFiles(string subtreeFile, string suchbaumFile)             
        {
            Console.WriteLine($"Processing subtree file: {subtreeFile}");
            Console.WriteLine($"Processing suchbaum file: {suchbaumFile}");

            var mainTree = new BinaryTree();

            // Read and build the suchbaum
            using (StreamReader reader = new StreamReader(suchbaumFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int key = int.Parse(line);
                    mainTree.Insert(key);
                }
            }

            // Read the search keys from the subtree file
            List<int> searchKeys = new List<int>();
            using (StreamReader reader = new StreamReader(subtreeFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    searchKeys.Add(int.Parse(line));
                }
            }

            foreach (int searchKey in searchKeys)
            {
                bool found = BinaryTree.PrintPathToKey(mainTree.Root, searchKey);
                if (!found)
                {
                    Console.WriteLine($"{searchKey} not found!");
                }
            }
        } */
    }
    static void ProcessFiles2(string suchbaumFile, string subtreeFile)
    {
        Console.WriteLine($"Processing suchbaum file: {suchbaumFile}");
        Console.WriteLine($"Processing subtree file: {subtreeFile}");
        var mainTree = new BinaryTree();

        // Read and build the suchbaum
        using (StreamReader reader = new StreamReader(suchbaumFile)) 
        {
            while (reader.ReadLine() is { } line)
            {
                int key = int.Parse(line);
                mainTree.Insert(key);
            }
        }

        // Read search keys and subtree keys
        string[] lines = File.ReadAllLines(subtreeFile);
        string[] subtreeKeys = lines[0].Split(',');

        // Check if subtree structure exists
        var subtreeStructure = new List<int>();
        foreach (string key in subtreeKeys)
        {
            subtreeStructure.Add(int.Parse(key));
        }

        bool subtreeFound = mainTree.CheckSubtreeStructure(mainTree.Root, subtreeStructure); 
        Console.WriteLine(subtreeFound ? "Subtree found" : "Subtree not found!");    
    }    
}
