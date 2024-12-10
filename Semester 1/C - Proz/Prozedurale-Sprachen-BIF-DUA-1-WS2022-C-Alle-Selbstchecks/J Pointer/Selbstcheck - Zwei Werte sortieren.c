#include <stdio.h>


void sort(int* p , int* q)
{
    if (*p > *q)
    {
    int temp = *q;
    *q = *p;
    *p = temp;
    }
    

}



int main()
{

    int x=0, y=0;
    int * p= &x, *q = &y;
    scanf("%d%d", p, q);
    sort(q, p);
    printf("%d\n%d", x,y);


    return 0;


}