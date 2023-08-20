#include <stdio.h>
#include <stdlib.h>

// Check if a number is a prime number
int checkprim(int maxx)
{
// loop through numbers from maxx-1 down to 2
    for (int x = maxx - 1; x > 2; x--)
    {
// if maxx is divisible by x, return 0 (not prime)
        if (maxx % x == 0) return 0;
    }
// if no divisor was found, return 1 (prime)
    return 1;
}

// Calculate all prime numbers up to max1
char* sieb(int max1)
{
// allocate memory for the array to store the results
    char* isPrimzahl = malloc(max1 + 2);

// loop through all numbers from 2 to max1
    for (int faktor = 2; faktor <= max1; faktor++)
    {
        // if the current number is not a prime, skip to the next number
        if (isPrimzahl[faktor] == '.') continue;
        // if the current number is prime, mark it as 'p'
        if (checkprim(faktor))
        {
            isPrimzahl[faktor] = 'p';
            // mark all multiples of the current prime as not prime
            for (int vielfaches = faktor * 2; vielfaches <= max1; vielfaches = vielfaches + faktor)
            {
                isPrimzahl[vielfaches] = '.';
            }
        }
    }

// return the array with the results
    return isPrimzahl;
}

int main()
{
    int max = 0;
// Input maximum number to check for prime numbers
    do
    {
        printf(": ");
        scanf("%d", &max);
    }
    while (max < 2 || max > 1000);

// Calculate all prime numbers up to max
    char* isPrimzahl = sieb(max);

// Print the prime numbers
    int count = 0;
    for (int i = 0; i <= max; ++i)
    {
        if (isPrimzahl[i] == 'p')
        {
            printf("%d ", i);
            if (++count % 10 == 0) printf("\n");
        }
    }

// Free memory used by isPrimzahl array
    free(isPrimzahl);
    return 0;
}
