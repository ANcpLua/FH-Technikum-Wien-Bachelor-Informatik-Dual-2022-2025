#include <stdio.h>

int main() {
    while(1) 
    {   
        int zahl;
        scanf("%d", &zahl);
        if(zahl<10) 
        {
        printf("%d\n", zahl);
        }
        else {
            printf("Abbruch");
        break; }

    } 
    return 0;
}