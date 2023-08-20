#include <stdio.h>
int main() {
    FILE* file;
    char filename[32];

    printf("Enter filename: ");
    scanf("%s", filename);

    file = fopen(filename, "r");


    if(file == NULL) {
        printf("Error opening file\n");
    } else {

        int x;
        while(fscanf(file, "value: %d\n", &x) != EOF)
            printf("x = %d\n", x);
        fclose(file);
    }
}
