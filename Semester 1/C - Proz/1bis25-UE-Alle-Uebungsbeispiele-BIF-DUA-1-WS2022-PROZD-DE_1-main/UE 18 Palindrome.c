// Hier werden zwei Bibliotheken eingebunden, die für die Funktionalität des Programms benötigt werden.
#include <stdio.h>
#include <ctype.h> // für Funktionen zur Verarbeitung von Zeichen, z.B. zur Änderung in Großbuchstaben

// Definition einer Konstanten, die die maximale Länge des Arrays 'message' festlegt
#define MAX_LEN 40

int main(void) 
{
// Definieren eines Arrays 'message' mit maximaler Länge MAX_LEN
char message[MAX_LEN];


// Definieren zwei Zeiger, die auf das erste und das letzte Element im Array zeigen 
char c, *p = message, *q;

// Schleife, die Zeichen einliest, solange sie noch nicht durch einen Zeilenumbruch beendet werden 
// oder die maximale Länge des Arrays erreicht ist.
while ((c = toupper(getchar())) != '\n' && p < message + MAX_LEN) 
{
    // Prüfen, ob es sich um einen Buchstaben handelt, und das Zeichen im Array speichern, wenn ja
    if (isalpha(c))
        *p++ = c;
}
// Zeiger auf das vorletzte Zeichen im Array setzen
p--;

// Schleife, die das Array von beiden Enden her überprüft, ob die Zeichen gleich sind
for (q = message; q < p; q++, p--) 
{
    // Falls ein ungleiches Zeichen gefunden wird, geben wir "false" aus und beenden das Programm
    if (*p != *q) 
	{
        printf(": false");
        return 0;
    }
}
// Ausgabe, wenn das Array ein Palindrom ist
printf(": true\n");
return 0;
}