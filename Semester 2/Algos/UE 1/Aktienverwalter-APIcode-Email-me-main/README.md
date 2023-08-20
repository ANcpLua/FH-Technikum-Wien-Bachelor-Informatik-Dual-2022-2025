
![Interface](https://user-images.githubusercontent.com/124206820/227749910-19de38c3-5046-41c4-bd73-370c77a671f0.png)

### Das Programm wird über folgende Menüpunkte gesteuert:
1. ADD: Eine Aktie mit Namen, WKN und Kürzel wird hinzugefügt.
2. DEL: Aktie wird gelöscht.
3. IMPORT: Kurswerte für eine Aktie werden aus einer csv Datei importiert
4. SEARCH: Eine Aktie wird in der Hashtabelle gesucht (Eingabe von Namen oder Kürzel) und der aktuellste Kurseintrag (Date,Open,High,Low,Close,Volume,Adj Close) wird ausgegeben.
5. PLOT: Die Schlusskurse der letzten 30 Tage einer Aktie werden als ASCII Grafik ausgegeben, Format ist frei wählbar.
6. SAVE <filename>: Programm speichert die Hashtabelle in eine Datei ab
7. LOAD <filename>: Programm lädt die Hashtabelle aus einer Datei
8. QUIT: Programm wird beendet

------------------------------------------------------------------------------------------------------------------------------------------------------


After downloading the .zip extract the files, press the .exe and it should work. If it does not follow the steps below.

Environment: PyCharm 2022.3.3
  
Manual:

1. Download the zip file by clicking on the green [<> Code] button. Once the download is finished, drag its contents into the PycharmProjects folder.
2. Open PyCharm and select the StockHandler-UE1-BIF-DUA-2-SS2023-ALGOS-DE-main project under Projects.
3. Press CTRL+ALT+S to open the settings window, and type "interpreter" into the search box in the top left corner.
4. Click on "Add Interpreter" and select "Add Local Interpreter".
5. Under "Virtualenv Environment", choose "Base interpreter: Python 3.10" and click "OK". Python 3.8+ should also work (if nothing works, download Anaconda3).
6. Click on the "+" icon (above "Package", below "Python Interpreter") and type "colorama" into the search box. Click on the "colorama" package and click "Install Package" located at the bottom left corner.
7. Close the "Available Packages" window and click on "Apply" and "OK" in the settings window. After that, you should be set up and ready to go.
8. Press Shift+F10 to start (recommended to change the hotkey: 1. Press CTRL+ALT+S 2. Type "run" into the search box 3. Right-click "Run" and select "Add Keyboard Shortcut").


Any questions you can contact me on Discord Alex22#8812
