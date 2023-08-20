#include <stdio.h>
#include <stdlib.h>

int myPow(int x, int y)
{
    int val = x;
    for(int n=1; n<y; ++n)
        val*=x;
    return val;
}

int add(int x, int y)
{
    return x+y;
}

int mult(int x, int y)
{
    return x*y;
}

int main()
{
    //INITIALIZE POINTER ARRAY
    int (*fPointers[3])(int, int) = {&myPow,&add,&mult};
    int choice;
    int x, y;
    do
    {
        printf("\nChoose function (0-2)");
        scanf("%d",&choice);

        if(choice < 0 || choice > 2)
            break;

        printf("\nEnter x");
        scanf("%d", &x);

        printf("\nEnter y");
        scanf("%d", &y);

        //CALL FUNCTION VIA POINTER ARRAY
        printf("\nResult: %d", (*fPointers[choice])(x, y));
    }
    while(1);

    return 0;
}
