#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#define PLUS1(x) (x) + 1
#define MINUS1(x) (x) - 1

int main() {
   
    int x;
    scanf("%d", &x);
    printf("%d\n", PLUS1(x));
    printf("%d\n", MINUS1(x));
    return 0;
 }