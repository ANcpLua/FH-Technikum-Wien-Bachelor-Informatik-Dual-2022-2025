#include <stdio.h>
#include <stdlib.h>
#include <string.h> 

int main()
{
    int satz[81];   // Array to store the frequency of word lengths 
    char z = 'Z';   // Character input from the user
    int len = 0;    // Variable to store the length of each word
    
    // Prompt user to enter input
    printf(": "); 
    
    // Initialize all elements of the satz array to 0
    for(int i = 0; i < 81; i++)
    {
        satz[i] = 0;
    }
    
    // Loop to read characters from the user
    for(int i = 1; i < 81; i++)
    {
        scanf("%c", &z);  // Read input from user
        
        // Check if the input character is an alphabet (upper or lower case)
        if((z >= 65 && z <= 90) || (z >= 97 && z <= 122))
        {
            len += 1;    // Increment the length of the current word
        }
        // Check if the input character is a space
        else if (z == ' ')
        {
            satz[len] += 1;   // Increment the frequency of words of length len
            len = 0;          // Reset the length of the current word
        }
        // Check if the input character is a period
        else if (z == '.')  
        {
            satz[len] += 1;   // Increment the frequency of words of length len
            len = 0;          // Reset the length of the current word
            
            // Loop to print the frequency of words of each length
            for(int y = 1; y < 81; y++)
            {
                if(satz[y] < 81 && satz[y] > 0)
                printf("Length %d: %d\n", y, satz[y]);
            }
            return 1;  // Exit the program
        }
        // If the input character is not an alphabet, space or period
        else 
        { 
            printf("NOT VALID");  // Print error message
            return 1;             // Exit the program
        }
    }
    // If the input does not contain a period
    printf("NOT VALID");   // Print error message
    return 1;             // Exit the program
}
