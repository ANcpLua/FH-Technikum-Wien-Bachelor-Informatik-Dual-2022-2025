#include <stdio.h> // include the standard input/output library
#include <stdlib.h> // include the standard library
#include <string.h> // include the string library
#include <stdbool.h> // include the boolean library

struct point // create a struct called "point"
{
double x, y; // this struct has two variables, x and y
} typedef point; // define point as a type

int main()
{
int temp; // variable for temporary storage during swapping
int schrit; // variable for counting steps
int weg; // variable for the length of the array
printf(": "); // prompt user to enter a value
scanf("%d", &weg); // read the value and store it in "weg"

schrit = weg - 1; // initialize "schrit" as one less than "weg"
int arr[weg]; // define an array with length "weg"

printf(": "); // prompt user to enter values

for (int i = 0; i < weg; ++i)
{
    scanf("%d", &arr[i]); // read values and store them in the array
}

for (int i = weg-1; i > 0; --i)
{
    if (arr[i] < arr[i-1]) // if the current value is less than the previous one
    {
        schrit--; // decrement "schrit"
    }
    else if (i < 0) // if "i" is negative
    {
        break; // break out of the loop
    }
    else
    {
        break; // break out of the loop
    }
}

if (schrit != 0) // if "schrit" is not equal to 0
{
    bool doBreak = false; // variable for controlling loop breaks

    for (int i = 1; arr[schrit - 1] + i < 10; ++i)
    {
        if (doBreak) // if "doBreak" is true
        {
            break; // break out of the loop
        }

        for (int j = schrit; j < weg; ++j)
        {
            if (arr[schrit - 1] + i == arr[j]) // if "arr[schrit - 1] + i" is equal to "arr[j]"
            {
                temp = arr[j]; // store "arr[j]" in "temp"
                arr[j] = arr[schrit - 1]; // swap values
                arr[schrit - 1] = temp;
                doBreak = true; // set "doBreak" to true
                break; // break out of the inner loop
            }
        }
    }
}

for (int j = schrit + 1; j < weg ; j++) // sort the array from "schrit + 1" to the end
{
    for (int j = schrit; j < weg - 1; j++) // inner loop for sorting
    {
        if (arr[j] > arr[j+1]) // if the current value is greater than the next one
        {
            temp = arr[j]; // store the current value in "temp"
             arr[j] = arr[j+1];
                arr[j+1] = temp;
                temp++;

            }
        }
}
    for(int j = schrit + 1; j < weg ; j++)
    {
        for(int j= schrit; j < weg-1; j++)
        {
            // swap the elements if the current one is larger than the next one
            if(arr[j] > arr[j+1])
            {
                temp = arr[j];
                arr[j] = arr[j+1];
                arr[j+1] = temp;
            }
        }
    }

    // print out the sorted array
    printf("\n");
    for (int i = 0; i < weg; ++i)
    {
        printf("%d ", arr[i]);
    }

    return 0;
}
