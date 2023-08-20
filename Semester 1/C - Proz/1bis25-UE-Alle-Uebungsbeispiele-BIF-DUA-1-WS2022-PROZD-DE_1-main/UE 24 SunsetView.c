#include <stdio.h>
#include <stdlib.h>

// Struct to store building height and its index
typedef struct buildings
{
    int height;
    int index;
    struct buildings *next; // Pointer to the next building in the linked list
} buildings;

// Function to free all the buildings in the linked list
void free_all(buildings *list)
{
    buildings *p, *np;
    p = list;
    while (p != NULL)
    {
        np = p->next; // Save the next building's address
        free(p); // Free the current building
        p = NULL;
        p = np; // Move to the next building
    }
}

// Function to create a new building with given height and index
buildings *create_building(int input, int index)
{
    buildings *new_building = malloc(sizeof(buildings));
    new_building->height = input;
    new_building->index = index;
    new_building->next = NULL;
    return new_building;
}

// Function to update the sunset view of the buildings
buildings *sunset_update(buildings *list, int input, int index)
{
    int first = 0;
    int counter = 0;
    buildings *prev = list;
    buildings *p = list;
    if (p == NULL) // If the list is empty, create the first building
        return create_building(input, index);
    else
    {
        while (p != NULL)
        {
            counter++;
            if (p->height <= input) // If a building with a smaller height is found, stop searching
                break;
            if (p->next == NULL) // If this is the last building in the list, add a new building to the end
            {
                p->next = create_building(input, index);
                return list;
            }
            p = p->next;
            if (first)
                prev = prev->next;
            if (!first)
                first = 1;
        }
        free_all(p); // Free all the buildings with a smaller height than the new building
        if (counter == 1) // If there was only one building in the list, return the new building
            return create_building(input, index);
        prev->next = create_building(input, index); // Add the new building to the list
    }
    return list;
}

// Function to print the buildings in the list
void print(buildings *list)
{
    buildings *p = list;
    while (p != NULL)
    {
        printf("%d [%d] ", p->height, p->index);
        p = p->next;
    }
    printf("\n");
}

int main()
{
    buildings *sunset = NULL; // Initialize the linked list
    int input = -1;
    int index = 0;
    while (42) // Infinite loop
    {
        printf(": ");
        scanf("%d", &input);
        if (input == 0)
            break;
        sunset = sunset_update(sunset, input, index); // Update the sunset view
        print(sunset); // Print the updated list
        index++;
        //
    }
    print(sunset);
    free_all(sunset);
}