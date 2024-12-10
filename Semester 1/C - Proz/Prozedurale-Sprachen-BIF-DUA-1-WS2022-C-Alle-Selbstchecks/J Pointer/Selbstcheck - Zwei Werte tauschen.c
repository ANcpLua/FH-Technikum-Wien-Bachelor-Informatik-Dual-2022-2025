#include <stdio.h>


int swap(int* p , int* q)
{
    int temp = *q;

    *q = *p;
    *p = temp;
    return 0;

}



int main()
{

    int x=0, y=0;
    int * p= &x, *q = &y;
    scanf("%d%d", p, q);
    swap(p, q);
    printf("%d\n%d", x,y);


    return 0;


}
