#include <stdio.h>

int main()
{
    /* Declare variables */
    int position;  /* User's position */
    int leader=1;  /* Current leader */
    int first=1;   /* First position */
    int i=1;       /* Counter for participants */
     
    /* Endless loop */
    while (1)
    {    
        /* Increment participant counter */
        ++i;
        
        /* Prompt user for position */
        printf(": ");
        scanf("%d", &position);

        /* Check if position is 0 */
        if (position==0)
        {
            /* Break out of loop */
            break;
        }
        
        /* Check if position is less than or equal to the first position */
        if (position<=first)
        {   
            /* Increment first position */
            first++;
            
            /* Print the first position */
            printf("position #1: %d\n", first);

        }

        /* Check if position is 1 */
        if (position==1)
        {      
            /* Print the new leader */
            printf("new leader: #%d\n", i);
            
            /* Update leader */
            leader=i;

        }
    }
    /* Print final first position */
    printf("final position #1: %d\n",first);
    /* Print winner */
    printf("winner: #%d",leader);
}