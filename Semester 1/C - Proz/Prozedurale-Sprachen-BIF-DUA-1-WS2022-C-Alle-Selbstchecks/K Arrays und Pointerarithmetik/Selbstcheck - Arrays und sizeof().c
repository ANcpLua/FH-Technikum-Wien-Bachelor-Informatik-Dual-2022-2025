#include <stdio.h>

int main()
{
    int x;
    scanf("%d", &x);
    int i[x];
    double d[x];
    char c[x];
    
    printf("%zu\n", sizeof(i));
    printf("%zu\n", sizeof(d));
    printf("%zu\n", sizeof(c));

    return 0;
    
}
