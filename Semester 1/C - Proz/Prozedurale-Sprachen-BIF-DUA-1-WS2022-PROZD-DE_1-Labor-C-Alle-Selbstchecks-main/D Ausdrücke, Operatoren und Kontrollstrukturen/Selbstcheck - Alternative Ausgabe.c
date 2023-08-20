#include <stdio.h>

int main ()
{
    int p=0;
    int u=0;
    scanf("%d \n", &p);
    scanf("%d \n", &u);
    int summe=p+u;
    
    if  (p<=50) {
        printf("Negativ");
    } else if (summe<70) {
        printf("Negativ");
    } else {
        printf("Positiv");
        
    } 
    return 0;
}