#include <stdio.h>


int fib(int x)
{
    if (x == 0) 
    {  
        printf("0\n");
        return 0;
    }
    if (x == 1) 
    {
    printf("1\n");
    return 1;
    }
    int zwischenergebnis = fib(x-1)+ fib(x-2);
    printf("%d\n", zwischenergebnis);
    return zwischenergebnis;
}




int main() 
{
    int x;
    scanf("%d", &x);
    printf("%d. Fibonnaci-Zahl = %d", x,fib(x));
    return 0;
}