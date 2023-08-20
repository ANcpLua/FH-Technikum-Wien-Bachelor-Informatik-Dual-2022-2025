#include <stdio.h>

int main() {
    
    char test;
    do {   
    
    printf("Abbrechen (j/n)?\n");
    scanf(" %c", &test );
    
    }
    while (test!='j');
   
    return 0;

}
