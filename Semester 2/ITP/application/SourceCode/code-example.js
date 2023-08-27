//macht nix, nur fürs bild zum ändern hier!

//Programm zum Zusammenzählen der Zahlen von 1 bis 5

//Eine Variable kann einen beliegen Wert speichern.
//In diesem Fall wird der Wert 0 gespeichert.
var ergebnis= 0; 

//Nun kommt eine Schleife, die eine Anweisung 
//mehrmals - hier fünf Mal - wiederholt.

/*Beim ersten Durchlauf der Schleife wird die Variable "ergebnis"
um die Variable "zahl" erhöht. Da in "zahl" der Wert 1 steht, wird
"ergebnis" um 1 erhöht. Im zweiten Durchlauf wird "ergebnis" 
um 2 erhöht. Dieser Vorgang wird wiederholt, bis 
die Variable "zahl" den Wert 5 erreicht hat.
*/

//   (Start;      Bedingung;     Erhöhung)
for (let zahl = 1; zahl <= 5; zahl = zahl + 1) {

//Die Variable "ergebnis" wird um die Variable "zahl" erhöht.
  ergebnis = ergebnis + zahl; 
}




