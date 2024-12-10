#include <stdio.h>
#include <stdlib.h>
#include <string.h>


int main(){
  FILE* csvfile;
 // int num1, num2; 
  char buffer[124]; // arbitrary size of one line
  printf("Enter filename: ");
  scanf("%s", buffer);
  csvfile=fopen(buffer, "r"); // open file
  if(csvfile == NULL){ // check if the file was opened
    printf("Error opening file\n");
  }
    else {
  while(fscanf(csvfile, "%s", &buffer[0]) != EOF){
    char* token = strtok(buffer, ","); // use strtok on the buffer
    printf("x/y = %s", token); // print the first part of the line
    token = strtok(NULL, ","); // get the next entry
    printf("/%s\n", token); // print the 2nd part
    /*while(fscanf(csvfile, "%d,%d", &num1, &num2) != EOF) printf("x/y = %d/%d\n", num1, num2); fclose(csvfile); // close the file again } return(0); }*/
  }
  fclose(csvfile); // close the file again
  }return(0);
}