#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Define a structure for a linked list node
struct wl_element
{
    char word[100]; // Array to store a word
    struct wl_element *next; // Pointer to next node
};

// Global variable to store the head of the linked list
struct wl_element *word_list = {0};

// Function to create a new node
struct wl_element *create(char *text)
{
// Allocate memory for the new node
    struct wl_element *new = malloc(sizeof(struct wl_element));
    if(new == NULL) // Return NULL if memory allocation fails
        return NULL;
    strcpy(new->word, text); // Copy the text to the node's word array
    new->next = NULL; // Set the next pointer to NULL
    return new;
}

// Function to attach a node to the end of the linked list
void attach_back(struct wl_element *element)
{
    if (word_list == NULL) // If the list is empty, set the head to the new node
    {
        word_list = element;
    }
    else // If the list is not empty, traverse to the end and attach the new node
    {
        struct wl_element *p = word_list;
        while (p->next != NULL)
        {
            p = p->next;
        }
        p->next = element;
    }
}

// Function to create a new node and attach it to the end of the linked list
struct wl_element* new_node(char *text)
{
    struct wl_element *new = create(text);
    if(new == NULL) // Return NULL if memory allocation fails
        return NULL;
    attach_back(new);
    return new;
}

// Function to detach the first node from the linked list
struct wl_element *detach_front()
{
    struct wl_element *p = word_list;
    if(p == NULL) // Return NULL if the list is empty
    {
        return NULL;
    }
    word_list = p->next; // Set the head to the second node
    p->next = NULL; // Set the detached node's next pointer to NULL
    return p;
}

// Function to free all nodes in the linked list
void free_all()
{
    if(word_list != NULL)
    {
        struct wl_element *p, *np;
        p = word_list;
        while(p!=NULL) // Traverse the list and free each node
        {
            np=p->next;
            free(p);
            p=np;
        }
    }
    word_list = NULL; // Set the head to NULL after freeing all nodes
}

// Function to rotate the linked list
void rotate(int count)
{
    for(int i = 0; i<count; i++)
    {
        struct wl_element *p = detach_front(); // Detach the first node
        attach_back(p); // Attach the node to the end of the list
    }

}

void print()
{
    // Gibt alle Einträge der Liste aus (siehe Beispiel).
    struct wl_element * p = word_list;
    while(p!=NULL)
    {
        printf("%s\n",p->word);
        p=p->next;
    }
}

int main()
{
    char buffer[100];
    int rot;

    do
    {
        scanf("%99s", buffer);
    }
    while ((buffer[0] != '-') && (NULL != new_node(buffer)));

    print();
    scanf("%d", &rot);
    rotate(rot);
    print();

    free_all();

    return 0;
}
