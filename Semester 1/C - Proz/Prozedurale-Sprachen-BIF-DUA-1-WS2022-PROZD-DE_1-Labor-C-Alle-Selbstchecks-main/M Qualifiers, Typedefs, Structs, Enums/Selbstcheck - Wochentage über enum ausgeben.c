#include <stdio.h>
#include <stdlib.h>


enum weekdays
{
sunday, monday, tuesday, wednesday,
thursday, friday, saturday
};


int main()
{   
    int wert;
    enum weekdays day;
    scanf("%d",&wert);
    day = wert;
    switch(day)
{
case 0:
    printf("sunday\n");
    break;
case 1:
    printf("monday\n");
    break;
case 2:
    printf("tuesday\n");
    break;
case 3:
    printf("wednesday\n");
    break;
case 4:
    printf("thursdas\n");
    break;
case 5:
    printf("friday\n");
    break;
case 6:
    printf("saturday\n");
    break;
default:
    printf("invalid\n");
    break;
}

    return 0;
}
