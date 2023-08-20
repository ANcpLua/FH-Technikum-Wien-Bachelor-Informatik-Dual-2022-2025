#include <stdio.h>
#include <stdlib.h>

typedef struct node Node;

struct node {
    int value;
    Node *next;
};

Node *createNode(int value) {
    Node *newNode = (Node *) malloc(sizeof(Node));
    newNode->value = value;
    return newNode;
}

Node *addFirst(Node *head, int value) {
    Node *newHead = createNode(value);
    newHead->next = head;
    return newHead;
}

void printList(Node *head) {
    Node *temp = head;
    while (temp != NULL) {
        printf("%d ", temp->value);
        temp = temp->next;
    }
    printf("\n");
}

Node *removeFirst(Node *head) {
    if (head == NULL) {
        return head;
    }
    Node *temp = head->next;
    free(head);
    return temp;
}

void freeList(Node *head) {
    Node *temp = head;
    while (temp != NULL) {
        temp = removeFirst(temp);
    }
}

Node *addLast(Node *head, int value) {
    if (head == NULL) {
        return addFirst(head, value);
    }
    Node *current = head;
    while (current->next != NULL) {
        current = current->next;
    }
    Node *newNode = createNode(value);
    current->next = newNode;
    return head;
}

Node *getNodeAt(Node *head, int index) {
    Node *current = head;
    for (int i = 0; i < index; i++) {
        current = current->next;
        if (current == NULL) {
            return NULL;
        }
    }
    return current;
}

Node *removeNodeIndex(Node *head, int index) {
    if (index == 0) {
        return removeFirst(head);
    }

    Node *preNode = getNodeAt(head, index - 1);
    Node *removeNode = preNode->next;
    Node *postNode = removeNode->next;

    preNode->next = postNode;

    free(removeNode);
    return head;
}

int  main() {
    Node *head = NULL;
    int input;
    while (1) {
        scanf("%d", &input);
        if (input == 0) {
            break;
        }
        head = addLast(head, input);
    }

    printList(head);

    while (1) {
        scanf(" %d", &input);
        if (input < 0) {
            break;
        }
        head = removeNodeIndex(head, input);
        printList(head);
    }

    freeList(head);
    return 0;
}