#include <stdio.h>
#include <stdlib.h>

int main()
{
    int s;
    int a=6;
    scanf ("%d", &s);
    scanf ("%d", &a);
    printf(": Surface area: %d\n",  a*s*s);
    printf("Volume: %d\n",  s*s*s);
    return 0;
}