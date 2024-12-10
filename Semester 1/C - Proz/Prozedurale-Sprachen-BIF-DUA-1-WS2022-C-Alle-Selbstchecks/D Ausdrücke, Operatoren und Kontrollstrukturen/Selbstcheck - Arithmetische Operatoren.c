#include <stdio.h>

int main()
{
    int x=0;
    int y=0;
    scanf("%d\n", &x);
    scanf("%d\n", &y);
    x=y+5;
    printf("%d\n",x);
    
    y=y*3;
    printf("%d\n",y);
    x=y/2;
    printf("%d\n",x);
    printf("%d\n",x%y);
    return 0;
    
}