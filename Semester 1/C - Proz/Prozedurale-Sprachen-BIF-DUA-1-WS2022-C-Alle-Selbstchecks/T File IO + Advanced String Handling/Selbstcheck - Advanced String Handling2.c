#define _XOPEN_SOURCE 500
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define INPUT_LEN 255
#define STR(s) STR2(s)
#define STR2(s) #s

typedef struct profile {
    size_t n;
    char **fields;
    struct profile *next;
} profile;

profile* createProfile(char* line) {
    profile* newProfile = malloc(sizeof(*newProfile));
    newProfile->n = 0;
    newProfile->fields = NULL;
    for(;;) {
        char *token = strtok(newProfile->fields ? NULL : line, ",");
        if(!token)
            break;
        newProfile->n++;
        char **tmp = realloc(newProfile->fields, (newProfile->n) * sizeof(*newProfile->fields));
        if(!tmp) {
            printf("realloc failed\n");
            free(newProfile->fields);
            free(newProfile);
            return NULL;
        }
        newProfile->fields = tmp;
        newProfile->fields[newProfile->n - 1] = strdup(token);
    }
    newProfile->next = NULL;
    return newProfile;
}

void printList(struct profile* head) {
    for(; head; head = head->next)
        for(size_t i = 0; i < head->n; i++)
            printf("%s%s", head->fields[i], i + 1 < head->n ? "/" : "\n");
}

struct profile* addLast(profile *head, profile *item) {
    if (!head)
        return item;
    profile *p = head;
    for(; p->next; p = p->next);
    p->next = item;
    return head;
}

int main(void) {
    printf("Enter filename: ");
    char pathname[FILENAME_MAX + 1];
    if(scanf("%" STR(FILENAME_MAX) "s", pathname) != 1) {
        printf("scanf failed\n");
        return 1;
    }

    FILE *file = fopen(pathname, "r");
    if(!file) {
        printf("Error opening file\n");
        return 1;
    }

    profile *head = NULL;
    char input[INPUT_LEN + 1];
    while(fscanf(file, "%" STR(INPUT_LEN) "s\n", input) != EOF) {
        head = addLast(head, createProfile(input));
    }
    fclose(file);
    printList(head);
}