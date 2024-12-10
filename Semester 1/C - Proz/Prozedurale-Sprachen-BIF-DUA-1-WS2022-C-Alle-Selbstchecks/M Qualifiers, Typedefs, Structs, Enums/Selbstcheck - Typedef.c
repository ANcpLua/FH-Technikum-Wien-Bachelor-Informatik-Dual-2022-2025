#include stdio.h
#include stdlib.h

int main()
{
    typedef volatile unsigned long long int vullint;
    vullint a=0,b=0,summe;
    scanf(%llun , &a);
    scanf(%llun , &b);
    summe = a + b;
    printf(%llu, summe);
    return 0;
}
