#include <stdio.h>

int main(){
    int note = 0;
    scanf("%d",&note);
    
    switch (note)
    {
        case 1: note=1;
            printf("Sehr gut");
            break;
        case 2: note=2;
            printf("Gut");
            break;
        case 3: note=3;
            printf("Befriedigend");
            break;
        case 4: note=4;
            printf("Genuegend");
            break;
        case 5: note=5;
            printf("Nicht genuegend");
            break;
        default:
            printf("Ungueltige Note\n");
            break;
    }
   return 0;
}