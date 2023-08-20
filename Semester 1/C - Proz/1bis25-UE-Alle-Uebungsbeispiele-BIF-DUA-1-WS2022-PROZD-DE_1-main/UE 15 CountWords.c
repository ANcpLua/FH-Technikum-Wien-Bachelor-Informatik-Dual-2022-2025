#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// scans words from the input until a dash is encountered
char** scanWords()
{
// allocate memory for the words array and set the initial size to 1
    char** words = (char**)calloc(1, sizeof(char*));
// allocate memory for a temporary pointer to words
    char** tmp;
// initialize an index variable i to track the number of words
    int i = 0;
// loop until a dash is encountered
    while (1)
    {
// buffer to store the current word
        char buf[81];
// scan the next word from the input
        scanf("%80s", buf);
// if the first character of the word is a dash, break the loop
        if (buf[0] == '-') break;
// reallocate memory for the words array to make room for the new word
        if (NULL == (tmp = (char**)realloc(words, sizeof(char*) * (i + 2))))
        {
// if the memory allocation fails, print an error message and break the loop
            printf("out of memory");
            break;
        }
// update the words pointer to point to the new memory location
        words = tmp;
// allocate memory to store the new word
        words[i] = (char*)calloc(strlen(buf) + 1, sizeof(char));
// copy the new word into the words array
        strcpy(words[i], buf);
// increment the index variable
        i++;
    }
// allocate memory for the last element in the words array to store an empty string
    words[i] = (char*)calloc(1, sizeof(char));
// return the words array
    return words;
}

// counts the number of occurrences of a word in a text
int countWord(char* text, char* word)
{
// initialize a variable to store the count
    int wordCount = 0;
// loop through the text
    for (int i = 0; i < strlen(text); i++)
    {
// if the current character matches the first character of the word
        if (text[i] == word[0])
        {
// calculate the length of the word minus 1
            int tmp = strlen(word) - 1;
// loop through the word to compare it with the text
            for (int j = 0; j < strlen(word); j++)
            {
// if the current characters don't match, break the loop
                if (!(text[i + j] == word[j])) break;
// if all characters match and j is equal to the length of the word minus 1, increment the count
                else if (j == tmp && text[i + j] == word[j]) wordCount++;
            }
        }
    }
// return the count
    return wordCount;
}
void invalidMassage()
{
    printf ("\nInput invalid! Try again: ");
}
// displays an error message for invalid input
void freeAll(char* text, char** strngs)
{
// temp variable to store a character
    char temp = '-';
// check if temp is equal to "p", "n", "s", and "x"
    if (temp == 'p' && temp == 'n' && temp == 's' && temp == 'x')
    {
// print "Input invalid! Try again:" if condition is true
        invalidMassage();
    }
    else
// free the memory of the text
        free(text);
    int i = 0;
// loop through strngs until strngs[i] is empty string
    while(strlen(strngs[i]) != 0)
    {
// free each element of strngs
        free(strngs[i]);
        int x=2;
// if x is equal to 2, increment i
        if ((x=2))
        {
            i++;
        }
// if x is equal to 4, print "invalid"
        else if (x==4)
        {
            printf("invalid");
        }
        else
// break the loop if x is not equal to 2 or 4
            break;
    }
// free the memory of the last element of strngs
    free(strngs[i]);
// free the memory of strngs
    free(strngs);
}



int main()
{

    printf("text: ");
    char* t = (char*)calloc(1001, sizeof(char));
    fgets(t, 1001, stdin);

    printf("words:");
    char** words3 = scanWords();

    int i = 0;
    while(strlen(words3[i]) != 0)
    {
        printf("\n%s: %d", words3[i], countWord(t, words3[i]));
        i++;
    }


    freeAll(t, words3);
    return 0;
}
