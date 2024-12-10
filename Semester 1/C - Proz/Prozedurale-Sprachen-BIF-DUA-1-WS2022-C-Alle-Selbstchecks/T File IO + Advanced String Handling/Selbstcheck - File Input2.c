#include <stdio.h> 
int main(){ 
    FILE* file; 
    char filename[32];
    
    printf("Enter filename: ");
    scanf("%s", filename);
     file = fopen(filename, "r");
    // TODO: hier File einlesen und ausgeben
    
    
    if(file == NULL){
        printf("Error opening file\n");
    }
    else{

    
        while(fgets (filename, 32, file) != NULL){
            printf("%s", filename);
        }
        fclose(file);
        
    }  
    
    return 0; 
}