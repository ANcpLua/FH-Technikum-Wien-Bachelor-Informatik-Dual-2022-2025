#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Liest die Eingabe des Benutzers ein und gibt einen Pointer auf ein Array von ints zurück
int* readInput(int* num_count)
{
    printf(": ");
    int go[1025], input, count, *nums;
    for(count = 0; count < 1025; count++)
    {
        scanf(" %d", &input);
        if (input == 0) break;
        go[count] = input;
    }
    num_count[0] = count;
    nums = (int*)malloc(sizeof(int)*count);
// Kopiere die eingelesenen Zahlen in das nums-Array
    for (int i = 0; i < count; i++)
    {
        nums[i] = go[i];
    }
    return nums;
}

// Gibt einen Schritt des Sortieralgorithmus aus
void printStep(int* nums_, int* pos)
{
    printf("\nStep %d: ", pos[2]+1);
    for (int i = 0; i < pos[0]; i++)
    {
        if (i == pos[1]) printf("[");
        if (i == pos[2]) printf("(");
        printf("%d", nums_[i]);
        if (i == pos[2]) printf(")");
        if (i == pos[1]) printf("]");
        printf(" ");
    }
}

// Sortiert das angegebene Array mit dem Selection Sort Algorithmus
void selection_sort(int* a, int* pos)
{
    int j, t, c = 0;
    for(pos[2] = 0; pos[2] < pos[0]-1; pos[2]++)
    {
        pos[1] = pos[2];
        for (j = pos[2]+1; j < pos[0]; j++)
        {
            c++;
            if (a[j] < a[pos[1]])
                pos[1] = j;
        }
        printStep(a, pos);
        t = a[pos[1]];
        a[pos[1]] = a[pos[2]];
        a[pos[2]] = t;
    }
    pos[3] = c;
}

int main()
{
    int pos[4] = {0};
    int* nums = readInput(pos);
    selection_sort(nums, pos);
    printf("\nResult: ");
    for (int i = 0; i < pos[0]; i++)
    {
        printf("%d ", nums[i]);
    }
    printf("\n\nNumber of comparisons: %d", pos[3]);
    printf("\nNumber of swaps: %d", pos[0]-1);
    free(nums);
    return 0;
}
