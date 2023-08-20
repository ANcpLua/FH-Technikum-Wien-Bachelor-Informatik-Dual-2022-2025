#include <stdio.h> 

int main()
{
    /* Initialize variables to store input, minimum, maximum, and average values */
    double zahl=0.0, max=0.0, min=0.0, avg=0.0; 
    /* Initialize a counter for the number of inputs */
    int menge = 0; 
    /* Prompt the user for the first input */
    printf(": "); 
    scanf("%lf", &zahl); 
    /* Initialize the maximum and minimum values to the first input */
    max = zahl; 
    min = zahl; 
    /* Add the first input to the average */
    avg += zahl; 

    /* Repeat the following block until the user enters a negative number */
    while(zahl > 0) 
    {
        /* Prompt the user for another input */
        printf(": "); 
        scanf("%lf", &zahl); 
        /* Update the maximum value if the current input is greater */
        if (zahl > max) 
        {
            max=zahl; 
        }
        /* Update the minimum value if the current input is less than the previous minimum and is positive */
        if (zahl < min && zahl > 0.0) 
        {
            min=zahl; 
        }
        /* Only add positive inputs to the average */
        if (zahl > 0.0) 
        {
            avg+=zahl; 
        }
        /* Increase the number of inputs */
        menge ++; 
    }
    /* If no positive numbers were entered, print "no number entered" */
    if (menge==0) 
    {
        printf("no number entered"); 
    }
    /* Otherwise, print the count, minimum, maximum, and average of the inputs */
    else 
    {
        /* Divide the total sum by the number of inputs to get the average */
        avg/=menge; 
        /* Print the count */
        printf("cnt: %d \n", menge); 
        /* Print the minimum */
        printf("min: %.2lf\n", min); 
        /* Print the maximum */
        printf("max: %.2lf\n", max); 
        /* Print the average */
        printf("avg: %.2lf\n", avg); 
    }
}
