#include <stdio.h>

int main () {
    
    int zahlen[6] = {0}, sum = 0, minSum = 0, maxSum = 0, kleinsteZahl = 0, groessteZahl = 0;
    // 5+1(0 terminator extra platz)
    scanf("%d %d %d %d %d", &zahlen[0], &zahlen[1], &zahlen[2], &zahlen[3], &zahlen[4]);
    // setup
    kleinsteZahl= zahlen[0];
    groessteZahl= zahlen[0];
    // schau alle zahlen an, und finde größte/kleinste
    for (int i=0; i < 5; i++) {
        if(groessteZahl < zahlen[i]){
            groessteZahl = zahlen[i];
        
        }
        
        if (kleinsteZahl > zahlen[i]) {
            kleinsteZahl = zahlen[i];
        }
         //summe aus 5 zahlen
        sum += zahlen[i];
    }

    // summe ohne kleinste zahl = maxSum und vise versa
    
    
    // summe aus den kleinsten 4 zahlen
    minSum = sum - groessteZahl;

    // summe aus den größten 4 zahlen
    maxSum = sum - kleinsteZahl;
    
    // ausgabe der ergebnisse
    printf(": minimum sum: %d \nmaximum sum: %d", minSum, maxSum);
    return 0;
    
}