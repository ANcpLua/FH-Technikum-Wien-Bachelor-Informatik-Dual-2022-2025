#include <stdio.h>

int main()
{
    int summe = 0;
    
    for(int i=0; i<3; ++i)
    {
        int zahl;
        scanf("%d", &zahl);
        summe += zahl;
    }
    
    printf("%d\n", summe);
    return 0;
}