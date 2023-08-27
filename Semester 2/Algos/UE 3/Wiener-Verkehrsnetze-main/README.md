

After downloading the .zip file, extract its contents and click on the .exe file. It should work. If it doesn't, follow the steps below.

Environment: PyCharm 2022.3.3 Manual:

1. Download the zip file by clicking on the green [<> Code] button. Once the download is complete, extract its contents into the PycharmProjects folder.
2. Open PyCharm and select Wiener Verkehrsnetz-UE3-BIF-DUA-2-SS2023-ALGOS-DE-main under Projects.
3. Press CTRL+ALT+S to open a window, then type "interpreter" into the top-left search box.
4. Click on "Add Interpreter" and select "Add Local Interpreter."
5. Under "Virtualenv Environment," choose "Base interpreter Python310" and click "OK." Versions 3.8 and above should also work.
6. Press Shift+F10 to start (it is recommended to change the hotkey). Press CTRL+ALT+S, type "run b" into the search box, right-click "Run" and select "Add Keyboard Shortcut."

    - [Beschreibung des verwendeten Algorithmus](#Beschreibung-des-verwendeten-Algorithmus)
    - [O-Notation](#O-Notation)
    - [Experimente und Messungen zur Laufzeit](#Experimente-und-Messungen-zur-Laufzeit)
  
## SHIFT+F10 in Pycharm  
```
Bitte geben Sie die Startstation ein: Praterstern
Bitte geben Sie die Zielstation ein: Hoechstaedtplatz
Start: Praterstern
Fahre von Praterstern nach Nordbahnstrasse mit Linie 5 (Kosten: 2)
Fahre von Nordbahnstrasse nach Am Tabor mit Linie 5 (Kosten: 4)
Umstieg auf Linie 2 an Station Rebhanngasse
Fahre von Am Tabor nach Rebhanngasse mit Linie 2 (Kosten: 10)
Fahre von Rebhanngasse nach Innstrasse mit Linie 2 (Kosten: 11)
Fahre von Innstrasse nach Traisengasse mit Linie 2 (Kosten: 12)
Fahre von Traisengasse nach Dresdner Strasse mit Linie 2 (Kosten: 14)
Fahre von Dresdner Strasse nach Hoechstaedtplatz mit Linie 2 (Kosten: 15)
Ende: Hoechstaedtplatz
Gesamtkosten: 15
Laufzeit: 0.00200 Sekunden
Möchten Sie weiterfahren (w), neu anfangen (n) oder beenden (x)? 

```  

#### Beschreibung des verwendeten Algorithmus
Wir verwenden den Dijkstra-Algorithmus, der ein gängiger Algorithmus zur Lösung des Problems des kürzesten Pfades in einem Graphen ist. Dieser Algorithmus beginnt an einem Startknoten und arbeitet sich schrittweise durch den Graphen, indem er immer den Knoten mit den geringsten Gesamtkosten (vom Startknoten aus) auswählt und zu einem bereits besuchten Pfad hinzufügt. Sobald der Zielknoten erreicht wird, gibt der Algorithmus den gefundenen Pfad zurück. Wir haben diesen Algorithmus leicht modifiziert, um eine zusätzliche "Strafe" für das Wechseln von Linien einzuführen. Das U-Bahn-Netzproblem stellt eine Situation dar, in der das Wechseln von Linien oft mit zusätzlichem Aufwand und Zeit verbunden ist. Daher ist es sinnvoll, Linienwechsel zu minimieren, und der Algorithmus berücksichtigt dies, indem er die Kosten für das Wechseln von Linien zu den Gesamtkosten hinzufügt.

#### O-Notation
Die Zeitkomplexität des Dijkstra-Algorithmus ist O((V+E) log V), wobei V die Anzahl der Knoten (Stationen) und E die Anzahl der Kanten (Verbindungen zwischen Stationen) im Graphen ist. Diese Komplexität ergibt sich aus zwei Hauptkomponenten:

O(|V|log|V|): Jeder der |V| Knoten kann maximal einmal in die Prioritätswarteschlange eingefügt und entfernt werden. Da die Prioritätswarteschlange als Binär-Heap implementiert ist, kostet das Einfügen und Entfernen eines Elements O(log|V|). Daher ergibt die Behandlung aller Knoten insgesamt O(|V|log|V|).

O(|E|log|V|): Für jede der |E| Kanten kann die Prioritätswarteschlange aktualisiert werden, wenn die Kante verarbeitet wird. Da eine Aktualisierung in der Prioritätswarteschlange ebenfalls O(log|V|) kostet, resultiert die Behandlung aller Kanten in O(|E|log|V|). Die Summe dieser beiden Komponenten ergibt die Gesamtzeitkomplexität O((V+E) log V). Die Raumkomplexität ist O(V), da wir Informationen für jeden Knoten in der Prioritätswarteschlange und in der Menge der besuchten Knoten speichern müssen. Die Effizienz des Algorithmus hängt stark von der Struktur des U-Bahn-Netzes ab. Bei einem dicht verbundenen Netz könnte der Algorithmus ineffizient sein, während er bei einem weniger dicht verbundenen Netz effizienter sein könnte.

#### Experimente und Messungen zur Laufzeit
Um die Laufzeit des Algorithmus zu messen, können verschiedene Experimente mit unterschiedlichen Eingabedateien und U-Bahn-Netzwerken durchgeführt werden. Die Laufzeit kann mithilfe der Python time-Bibliothek gemessen werden, indem die Start- und Endzeit vor und nach der Ausführung des Algorithmus erfasst werden. Dabei sollte beachtet werden, dass die tatsächliche Laufzeit von verschiedenen Faktoren abhängt, wie z.B. der Größe des Netzwerks, der Anzahl der Umstiege und der Hardware des Computers, auf dem der Code ausgeführt wird.

Any questions you can contact me on Discord Alex22#8812

