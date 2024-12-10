#include <stdlib.h>
#include <stdio.h>

struct node {
    int element;
    struct node *next;
};
struct node* addFirst(int value, struct node* head) 
{
    struct node* newHead = (struct node*)malloc(sizeof(struct node)); 
    newHead->element = value; 
    newHead->next = head; 
    return newHead; 
}

struct node *removeFirst(struct node *head) {
    if (head == NULL) {
        return head;
    }
   struct node *temp = head->next;
    free(head);
    return temp;
}
struct node *deleteList(struct node *head) {
    struct node *temp = head;
    while (temp != NULL) {
        temp = removeFirst(temp);
    }
    return temp;
}

void printList(struct node *head) {
    struct node *temp = head;
    while (temp != NULL) {
        printf("%d ", temp->element);
        temp = temp->next;
    }
}

int main() {
    struct node *head = NULL;
    while(1) {
        int input;
        scanf("%d", &input);
        if (input == 0) {
            break;
        }
        head = addFirst(input, head);
    }

    printList(head);

    head = deleteList(head);
    return 0;

}


    
