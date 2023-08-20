#include<stdio.h>

int main(void)
{
    // Declaration of variables to store the number of days in the month and the starting day of the week
    int mtag=0;
    int wtag=0;
    int i;

    // Asking the user to input the number of days in the month
    printf("number of days in month: ");
    scanf("%d", &mtag);

    // Asking the user to input the starting day of the week (1=Monday, 7=Sunday)
    printf("starting day of week (1=Monday, 7=Sunday): ");
    scanf("%d", &wtag);

    // Adding a newline character
    printf("\n");

    // Loop to print empty spaces before the first day of the month
    for(i = 1 ; i < wtag; i++) 
    {
        printf("   ");
    }

    // Loop to print the days of the month
    for(i = 1; i <= mtag; i++) 
    {
        printf("%2d ", i);
        // If the current day plus the starting day of the week minus 1 is divisible by 7, move to a new line
        if((i + wtag - 1)%7 ==0) 
        {
            printf("\n");
        }
    }
    // Return 0 to indicate that the program executed successfully
    return 0;
}



