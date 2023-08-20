#include <stdio.h> // Import the standard input/output library
#include <stdlib.h> // Import the standard library

struct point
{
    double x, y; // Create a struct point with two double variables x and y
} typedef point; // Alias the struct point as point

int main()
{
    char array[100]; // Create an array of 100 characters
    char direction = 0; // Initialize a character direction
    char letter = 'A'; // Initialize a character letter to 'A'
    int x = 0; // Initialize an integer x to 0
    int y = 0; // Initialize an integer y to 0
    int space = 1; // Initialize an integer space to 1

// Fill the array with dots (.)
    for(int i = 0; i < 10; ++i)
    {
        for(int j = 0; j < 10; ++j)
        {
            array[i*10+j] = '.';
        }
    }
// Change the first element of the array to 'A'
    array[0] = 'A';

    do
    {
        // Print the array
        if(space)
        {
            for(int i = 0; i < 10; ++i)
            {
                for(int j = 0; j < 10; ++j)
                {
                    printf("%c", array[i*10+j]);
                }
                printf("\n");
            }
            // Increment the letter, if it's Z, change it to A
            if(letter == 'Z')
            {
                letter = 'A';
            }
            else
            {
                letter++;
            }
        }

        // Reset the space value to 1
        space = 1;
        // Read the direction from the user
        printf("walk to: ");
        scanf(" %c", &direction);
        printf("\n");

        // Check if direction is north, and update the position if possible
        if (direction=='n')
        {
            if ((y-1 < 0) || (array[((y-1)*10)+x] != '.') )
            {
                printf("not possible\n");
                space = 0; // Set the space value to 0 if movement is not possible
            }
            else
            {
                y--;
                array[((y)*10)+x] = letter;
            }
        }
        // Check if direction is east, and update the position if possible
        else if (direction=='e')
        {
            if ((x+1 > 9) || (array[(y*10)+(x+1)] != '.'))
            {
                printf("not possible\n");
                space = 0; // Set the space value to 0 if movement is not possible
            }
            else
            {
                x++;
                array[(y*10)+(x)] = letter;
            }
        }
        // Check if direction is south, and update the position if possible
        else if (direction=='s')
        {
            if ((y+1 > 9) || (array[((y+1)*10)+x] != '.'))
            {
                printf("not possible\n");
                space = 0; // Set the space value to 0 if movement is not possible
            }
            else
            {
                y++;
                array[((y)*10)+x] = letter;
            }
        }
        // Check if direction input is 'w' (West)
        else if (direction == 'w')
        {
// Check if new position is out of bounds (x-1 < 0) or if the new position is not an empty space ('.')
            if ((x - 1 < 0) || (array[(y * 10) + (x - 1)] != '.'))
            {
                printf("not possible\n");
// Set the space value to 0 if movement is not possible
                space = 0;
            }
// If new position is within bounds and is an empty space
            else
            {
                x--;
// Update the array with the new position
                array[(y * 10) + (x)] = letter;
            }
        }
    }
    while (direction != 'x');   // Repeat the loop as long as the input direction is not 'x' (Exit)
    return 0; // End the program with a return value of 0 (indicating successful execution)
}
