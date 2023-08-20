# Abschlusstest-C-70P

BSP: 1

Erweitern Sie folgendes Programm sodass:

Die eingegebenen Werte, wenn möglich, an der entsprechenden Stelle (beginnend bei 1) der Liste/Array eingefügt werden (Eingabe 1 wird an der ersten Stelle, Eingabe 2 wird an der zweiten Stelle, Eingabe 5 an der fünften Stelle und so weiter eingefügt). Gibt es die entsprechende Stelle nicht wird die Eingabe am Ende eingefügt (geben Sie nach jeder Eingabe die derzeitige Liste/Array aus).
Nach dem Einlesen aller Eingaben und der Ausgabe der Liste/Array die Summe der Werte die korrekt an ihrer Stelle eingefügt wurden ausgegeben wird.
Die Speicherverwaltung korrekt durchgeführt wird.
Sie können diese Aufgabe entweder mit einer verketteten Liste oder einem Array lösen. Verwenden Sie ein Array, so sollte es mit unendlich vielen Eingaben kompatibel sein und dynamisch erweitert werden (nicht von Anfang an ein Array der Größe 999 anlegen).

Hier ein Beispiel des Programmablaufs:


Input: 5
5
Input: 1
1-5
Input: 3
1-5-3
Input: 2
1-2-5-3
Input: 7
1-2-5-3-7
Input: 0

Correct numbers sum: 6


```Punkteschlüssel:```

#15 - Wert bei entsprechender Stelle einfügen

10 - Wert ansonsten am Ende einfügen 

5 - Liste nach jeder Eingabe ausgeben

5 - Anzahl der korrekt eingegebenen Werte ausgeben

5 - Speicherverwaltung
```
#include <stdio.h>
#include <stdlib.h>

/* uncomment for list solution */
/*
typedef struct Node {
    int n;
    struct Node* next;
} Node;

void printList(Node* head) {
    while (head != NULL) {
        printf("%d", head->n);
        if(head->next != NULL)
            printf("-");
        head = head->next;
    }
    printf("\n");
}
*/

/* uncomment for array solution */
/*
void printArray(int arr[], int length)
{
    for(int n=0; n<length; ++n)
    {
        printf("%d", arr[n]);
        if(n<length-1)
            printf("-");
    }
    printf("\n");
}
*/

int main() {
    int n;

    do {
        printf("Input: ");
        scanf("%d", &n);
        if (n > 0) {
            // VALUE INSERTION

            // PRINT LIST/ARRAY
        }
    } while (n > 0);

    // PRINT STATS

    return 0;
}

```  



////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


BSP 2:

Erweitern Sie das Programm sodass:

Die Funktion maxChar das am häufigsten vorkommende Zeichen eines Strings ermittelt.
Sind mehrere Zeichen am häufigsten vertreten wird das erste dieser Zeichen genommen.
Wird ein leerer String übergeben wird ein Leerzeichen als häufigstes Zeichen zurückgegeben.

```Punkteschlüssel:```

10 - Häufigsten Char ermitteln

5 - Erstes häufigstes Zeichen zurückgeben

5 - Leerzeichen bei leerem String zurückgeben
```
#include <unistd.h>
#include <sys/types.h>
#include <stdio.h>
#include <stdlib.h>

int main ()
{
    char input[50] = "...";

    while(1)
    {
        scanf("%49s", input);
        if(input[1] == '\0') //Terminates if only one letter was entered
            return 0;
        printf("Most common char for %s is: %c\n", input, maxChar(input));
    }
}

```  


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

BSP 3:

Ein Array von Funktionspointern angelegt wird und mit den drei Funktionen myPow, add und mult (in dieser Reihenfolge) befüllt wird.
Nach den Eingaben die Funktion entsprechend dem Wert in choice mit den Parametern x und y aufgerufen wird und das Resultat mit "Result: " ausgegeben wird.

```Punkteschlüssel:```

5 - Array anlegen

5 - Funktionsaufruf über das Array
```
#include <stdio.h>
#include <stdlib.h>

int myPow(int x, int y)
{
    int val = x;
    for(int n=1; n<y; ++n)
        val*=x;
    return val;
}

int add(int x, int y)
{
    return x+y;
}

int mult(int x, int y)
{
    return x*y;
}

int main()
{
    //INITIALIZE POINTER ARRAY

    int choice;
    int x, y;
    do{
        printf("\nChoose function (0-2)");
        scanf("%d",&choice);

        if(choice < 0 || choice > 2)
            break;

        printf("\nEnter x");
        scanf("%d", &x);

        printf("\nEnter y");
        scanf("%d", &y);

        //CALL FUNCTION VIA POINTER ARRAY
    }while(1);

    return 0;
}
```  
