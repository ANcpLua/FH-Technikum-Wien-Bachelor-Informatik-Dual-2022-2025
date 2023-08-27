import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.stream.Collectors;


class TreeNode {
    public int key;
    public TreeNode left;
    public TreeNode right;
}

class BinaryTree {
    public TreeNode root;

    public void insert(int key) {
        root = insert(root, key);
    }

    private TreeNode insert(TreeNode node, int key) {
        if (node == null) {
            TreeNode newNode = new TreeNode();
            newNode.key = key;
            return newNode;
        }

        if (key < node.key) {
            node.left = insert(node.left, key);
        } else if (key > node.key) {
            node.right = insert(node.right, key);
        }

        return node;
    }

    private int height(TreeNode node) {
        if (node == null) {
            return 0;
        }

        int leftHeight = height(node.left);
        int rightHeight = height(node.right);

        return Math.max(leftHeight, rightHeight) + 1;
    }

    private int balanceFactor(TreeNode node) {
        if (node == null) {
            return 0;
        }

        return height(node.right) - height(node.left);
    }

    public void traverseAndCheckAVL(TreeNode node, BooleanWrapper isAVL) {
        if (node == null) {
            return;
        }

        traverseAndCheckAVL(node.right, isAVL);
        traverseAndCheckAVL(node.left, isAVL);

        int balance = balanceFactor(node);
        System.out.print("bal(" + node.key + ") = " + balance);
        if (balance > 1 || balance < -1) {
            System.out.print(" (AVL violation!)");
            isAVL.value = false;
        }
        System.out.println();
    }

    public List<Integer> traverseAndCollectKeys(TreeNode node) {
        if (node == null) {
            return new ArrayList<>();
        }

        List<Integer> keys = new ArrayList<>();
        keys.addAll(traverseAndCollectKeys(node.left));
        keys.add(node.key);
        keys.addAll(traverseAndCollectKeys(node.right));
        return keys;
    }

    public boolean printPathToKey(TreeNode node, int key, List<Integer> path) {
        if (node == null) {
            return false;
        }

        if (path == null) {
            path = new ArrayList<>();
        }

        path.add(node.key);

        if (node.key == key) {
            System.out.println(key + " found " + path.toString());
            return true;
        }

        boolean foundInLeft = false;
        boolean foundInRight = false;
        if (key < node.key) {
            foundInLeft = printPathToKey(node.left, key, path);
        } else {
            foundInRight = printPathToKey(node.right, key, path);
        }

        path.remove(path.size() - 1);

        return foundInLeft || foundInRight;
    }

    public boolean checkSubtreeStructure(TreeNode node, List<Integer> subtreeStructure) {
        if (subtreeStructure.isEmpty()) {
            return true;
        }
        if (node == null) {
            return false;
        }

        int index = subtreeStructure.indexOf(node.key);
        if (index == 0) {
            subtreeStructure.remove(0);
        }

        return checkSubtreeStructure(node.left, subtreeStructure) ||
                checkSubtreeStructure(node.right, subtreeStructure);
    }
}

class BooleanWrapper {
    public boolean value;
}

public class Main {
    public static void main(String[] args) throws IOException {

        processFile1(args[0]);
        processFiles(args[1], args[2]);
        processFiles2(args[1], args[2]);
    }

    static void processFile1(String filename) throws IOException {
        System.out.println("Processing file: " + filename);

        BinaryTree tree = new BinaryTree();

        try (BufferedReader reader = new BufferedReader(new FileReader(filename))) {
            HashSet<Integer> addedKeys = new HashSet<>();
            String line;
            while ((line = reader.readLine()) != null) {
                int key = Integer.parseInt(line);
                if (!addedKeys.contains(key)) {
                    tree.insert(key);
                    addedKeys.add(key);
                }
            }
        }

        BooleanWrapper isAVL = new BooleanWrapper();
        isAVL.value = true;
        tree.traverseAndCheckAVL(tree.root, isAVL);
        System.out.println("AVL: " + (isAVL.value ? "yes" : "no"));

        List<Integer> keys = tree.traverseAndCollectKeys(tree.root);
        double avg = keys.stream().mapToInt(Integer::intValue).average().orElse(0);
        System.out.println("min: " + keys.stream().mapToInt(Integer::intValue).min().orElse(0)
                + ", max: " + keys.stream().mapToInt(Integer::intValue).max().orElse(0)
                + ", avg: " + String.format("%.1f", avg));
    }

    static void processFiles(String subtreeFile, String suchbaumFile) throws IOException {
        System.out.println("Processing subtree file: " + subtreeFile);
        System.out.println("Processing suchbaum file: " + suchbaumFile);

        BinaryTree mainTree = new BinaryTree();

        try (BufferedReader reader = new BufferedReader(new FileReader(suchbaumFile))) {
            String line;
            while ((line = reader.readLine()) != null) {
                int key = Integer.parseInt(line.trim()); // Add trim() here
                mainTree.insert(key);
            }
        }

        int searchKey;
        try (BufferedReader reader = new BufferedReader(new FileReader(subtreeFile))) {
            String line = reader.readLine();
            if (line == null) {
                System.out.println("No search key provided in the subtree file!");
                return;
            }
            searchKey = Integer.parseInt(line.trim()); // Add trim() here
        }

        boolean found = mainTree.printPathToKey(mainTree.root, searchKey, null);
        if (!found) {
            System.out.println(searchKey + " not found");
        }
    }

    static void processFiles2(String suchbaumFile, String subtreeFile) throws IOException {
        System.out.println("Processing suchbaum file: " + suchbaumFile);
        System.out.println("Processing subtree file: " + subtreeFile);

        BinaryTree mainTree = new BinaryTree();

        try (BufferedReader reader = new BufferedReader(new FileReader(suchbaumFile))) {
            String line;
            while ((line = reader.readLine()) != null) {
                int key = Integer.parseInt(line.trim());
                mainTree.insert(key);
            }
        }

        String[] lines = Files.readAllLines(Paths.get(subtreeFile)).toArray(new String[0]);
        String[] subtreeKeys = lines[0].trim().split("\\s*,\\s*");

        List<Integer> subtreeStructure = new ArrayList<>();
        for (String key : subtreeKeys) {
            subtreeStructure.add(Integer.parseInt(key.trim()));
        }

        boolean subtreeFound = mainTree.checkSubtreeStructure(mainTree.root, subtreeStructure);
        System.out.println("Subtree structure found: " + (subtreeFound ? "yes" : "no"));
    }
}

   
