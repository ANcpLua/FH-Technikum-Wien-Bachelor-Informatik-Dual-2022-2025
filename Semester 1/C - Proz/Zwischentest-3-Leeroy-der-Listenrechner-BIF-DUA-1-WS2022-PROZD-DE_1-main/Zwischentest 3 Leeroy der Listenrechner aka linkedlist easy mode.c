#include <stdio.h>
#include <stdlib.h>
#include <string.h>

enum node_type {number_type = 1, operator_type, parenthesis_type};
struct node
{
    char operator;
    int number;
    enum node_type type;
    struct node* next;
};

struct node* createNode(void)
{
    // create a new node by setting next value as NULL
    struct node* newNode = (struct node*)malloc(sizeof(struct node));
    newNode->next = NULL;
    return newNode;
}

struct node* createParenthesisNode(char op)
{
    struct node* newNode = createNode();
    // set operator
    newNode->operator = op;
    // set type
    newNode->type = parenthesis_type;
    return newNode;
}

struct node* createOperatorNode(char op)
{
    struct node* newNode = createNode();
    newNode->operator = op;
    newNode->type = operator_type;
    return newNode;
}

struct node* createNumberNode(int num)
{
    struct node* newNode = createNode();
    //struccctttttt
    newNode->number = num;
    newNode->type = number_type;
    return newNode;
}

struct node *inputToNode(const char *input)
{
    int number = atoi(input);
    if (number != 0)
    {
        return createNumberNode(number);
    }
    if (input[0] == '0')
    {
        return createNumberNode(0);
    }
    if (strcmp(input, "(") == 0 ||strcmp(input, ")") == 0)
    {
        return createParenthesisNode(input[0]);
    }
    if (strcmp(input, "+") == 0 || strcmp(input, "-") == 0 ||strcmp(input, "*") == 0 ||strcmp(input, "/") == 0)
    {
        return createOperatorNode(input[0]);
    }
    return NULL;
}

struct node *addLast(struct node *head, struct node *newNode)
{
    // if head is NULL, return the newNode as head
    if (head == NULL)
        return newNode;

    struct node* temp = head;
    // loop till the last node is reached
    while (temp->next)
    {
        temp = temp->next;
    }
    // set the next node as newNode
    temp->next = newNode;
    return head;
}

struct node *findLastParenthesisOpen (struct node *head)
{
    struct node* last = NULL;
    // loop through all the nodes
    while (head)
    {
        // if the type is parenthesis and operator is open parenthesis, the last is replaced by the current node
        if (head->type == parenthesis_type)
        {
            if (head->operator == '(')
                last = head;
        }
        head = head->next;
    }
    return last;

}

struct node *findFirstPointOperator(struct node *startNode)
{
    while (startNode)
    {
        // if there is a parenthesis, stop searching
        if (startNode->type == parenthesis_type)
            return NULL;
        if (startNode->type == operator_type)
        {
            if (startNode->operator == '*' || startNode->operator == '/')
                return startNode;
        }
        startNode = startNode->next;
    }
    return NULL;
}

struct node *findFirstDashOperator(struct node *startNode)
{
    while (startNode)
    {
        if (startNode->type == operator_type)
        {
            if (startNode->operator == '+' || startNode->operator == '-')
                return startNode;
        }
        startNode = startNode->next;
    }
    return NULL;
}

struct node *findPrevious (struct node *head, struct node *node)
{
    // loop through all the nodes
    while (head)
    {
        // if next of the current node is equal to the provided node
        if (head->next == node)
        {
            return head;
        }
        head = head->next;
    }
    return NULL;
}

// print node by looping through all the nodes
void printNode(struct node* head)
{
    while(head)
    {
        if (head->type == number_type)
        {
            printf("%d", head->number);  // print int
        }
        else
        {
            printf("%c", head->operator); // print char
        }
        head = head->next;
    }
    printf("\n");
}

struct node* evaluate(struct node* head)
{
    if (head == NULL)
        return NULL;
    struct node* prev, *temp, *dividenull;
    // assign the current node by head
    struct node* current = head;
    // find open parenthesis
    struct node* parenthesis = findLastParenthesisOpen(head);
    if (parenthesis)
    {
        // if there are (x) in expression, remove both parenthesis
        if (parenthesis->next->next->operator == ')')
        {
            temp = parenthesis->next->next->next;
            free(parenthesis->next->next);
            parenthesis->next->next = temp;
            temp = parenthesis->next;
            if (parenthesis==head)
            {
                current = head = temp;
            }
            else
            {
                prev = findPrevious(head, parenthesis);
                prev->next = temp;
            }
            free(parenthesis);
            parenthesis = findLastParenthesisOpen(current);
            if (parenthesis)
                current = parenthesis->next;
        }
        else
            // replace the current head as next of parenthesis
            current = parenthesis->next;
    }


    // find the point operator
    struct node* point = findFirstPointOperator(current);

    // if the point operator exits, calculates the expression by finding 2 operands
    if (point)
    {
        prev = findPrevious(head, point);
        dividenull = (point->next);
        if (point->operator == '*')
        {
            prev->number = prev->number * point->next->number;
        }
        else
        {
            dividenull->number = dividenull->number == 0 ? 1 : dividenull->number;  // inspired by ben10 zt1 Ã¡â€¢â€¢( Ã¡Ââ€º )Ã¡â€¢â€”
            prev->number = prev->number / point->next->number;
        }
    }
    else
    {
        // if no point operator exists, dash operation will be performed
        struct node *dash = findFirstDashOperator(current);
        if (dash)
        {
            prev = findPrevious(head, dash);

            if (dash->operator == '+')
            {
                prev->number = prev->number + dash->next->number;
            }
            else
                prev->number = prev->number - dash->next->number;
        }
        else
            return NULL;
    }
    // remove operator and the second operand nodes
    temp = prev->next->next->next;
    free(prev->next->next);
    free(prev->next);
    prev->next = temp;
    return head;
}

int main()
{
    struct node* head = NULL;
    char str[100];
    printf("\n");
    while (1)
    {
        printf("Input:");
        scanf("%s", str);
        if (str[0] == '=')
        {
            printf("\n");
            break;
        }
        struct node* newNode = inputToNode(str);
        head = addLast(head, newNode);
        printf("\nTerm: ");
        printNode(head);
    }
    while (1)
    {
        head = evaluate(head);
        printf("Resulting term: ");
        printNode(head);
        if (head->next == NULL)
            break;
    }
    free(head);

    return 0;
}
