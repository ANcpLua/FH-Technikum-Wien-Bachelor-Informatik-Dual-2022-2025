#include <stdio.h>

int power(int x,int y)
{
    if (y==0)
    return 1;
    int zwischenergebnis= power (x,y-1)*x;
    printf("%d\n", zwischenergebnis);
    return zwischenergebnis;
}


int main()
{
    int x,y;
    scanf("%d%d", &x, &y);
    power(x,y);
    return 0;
}