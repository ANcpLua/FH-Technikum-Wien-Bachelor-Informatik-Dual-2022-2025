#include <stdio.h>
#include <string.h>
#include <unistd.h>

char maxChar(char input[50]) 
{
    int charCount[256] = {0};
    int maxAppear = 0;
    char max = ' ';
    int length = strlen(input);

    for (int i = 0; i < length; i++) 
    {
        charCount[input[i]]++;
        if (charCount[input[i]] > maxAppear) 
	{
            max = input[i];
            maxAppear = charCount[input[i]];
        }
    }

    return max;
}

int main()
{
    char input[50] = "...";
    while (1) 
    {
        scanf("%49s", input);
        if (input[1] == '\0') //Terminates if only one letter was entered
            return 0;
        printf("Most common char for %s is: %c\n", input, maxChar(input));
    }
    return 0;
}
