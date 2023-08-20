#include <stdio.h>
#include <stdlib.h>

typedef struct Node
{
    int n;
    struct Node* next;
} Node;

struct Node* addLast(struct Node* head, int n)
{
    struct Node* new_node = (struct Node*) malloc(sizeof(struct Node));
    new_node->n = n;
    new_node->next = NULL;

    if (head == NULL)
    {
        return new_node;
    }

    struct Node* temp = head;
    while (temp->next != NULL)
    {
        temp = temp->next;
    }
    temp->next = new_node;

    return head;
}

struct Node* addAt(struct Node* head, int n)
{
    struct Node* new_node = (struct Node*) malloc(sizeof(struct Node));
    new_node->n = n;

    if (head == NULL)
    {
        new_node->next = NULL;
        return new_node;
    }

    if (head->n >= n)
    {
        new_node->next = head;
        return new_node;
    }

    struct Node* temp = head;
    while (temp->next != NULL && temp->next->n < n)
    {
        temp = temp->next;
    }
    new_node->next = temp->next;
    temp->next = new_node;

    return head;
}

void printList(Node* head)
{
    while (head != NULL)
    {
        printf("%d", head->n);
        if(head->next != NULL)
            printf("-");
        head = head->next;
    }
    printf("\n");
}

void deleteList(struct Node* head)
{
    struct Node* temp = head;
    while (temp != NULL)
    {
        struct Node* next = temp->next;
        free(temp);
        temp = next;
    }
}

int main()
{
    int n;
    int sum = 0;
    struct Node* head = NULL;

    printf("Input: ");
    while (scanf("%d", &n) == 1 && n > 0)
    {
        if (head == NULL || n > head->n)
        {
            head = addLast(head, n);
            sum += n;
        }
        else
        {
            head = addAt(head, n);
            sum += n;
        }
        printList(head);
        printf("Input: ");
    }

    printf("\nCorrect numbers sum: %d", sum);
    deleteList(head);
    return 0;
}
