TREE

```-ALGOS-DE-main/Treealgos/bin/Debug/net7.0/Treealgos.exe filename.txt filename-suchbaum.txt filename-subtree.txt
Processing file: filename.txt
bal(79) = 0
bal(30) = 0
bal(54) = 0
bal(23) = 2 (AVL violation!)
bal(12) = 0
bal(11) = 1
bal(9) = 2 (AVL violation!)
bal(17) = 0
bal(3) = 0
bal(5) = 3 (AVL violation!)
AVL: no
min: 3, max: 79, avg: 24.3
Processing subtree file: filename-suchbaum.txt
Processing suchbaum file: filename-subtree.txt
7 found 5, 8, 7
Processing suchbaum file: filename-suchbaum.txt
Processing subtree file: filename-subtree.txt
Subtree not found!
```


Environment: JetBrains Rider 2022.3.2

Manual:

1. Download the zip file by clicking on the green [<> Code] button. Once the download is complete, extract its contents into the RiderProjects folder.
2. Open Rider and click on "Open." Select the RiderProjects\TREE-UE2-BIF-DUA-2-SS2023-ALGOS-DE-main folder that you extracted earlier.
3. In the top right corner, click on "treealgos" and select "Edit Configurations."
4. Search for "Program arguments" and enter "filename.txt filename-suchbaum.txt filename-subtree.txt" in the field.
5. Click "Apply" and press CTRL+F5.
6. The program should now work and display results in the terminal. If you want to change the keys, modify the integers inside filename-subtree.txt while maintaining the format (newline-separated).

If you have any questions, feel free to contact me on Discord: Alex22#8812


