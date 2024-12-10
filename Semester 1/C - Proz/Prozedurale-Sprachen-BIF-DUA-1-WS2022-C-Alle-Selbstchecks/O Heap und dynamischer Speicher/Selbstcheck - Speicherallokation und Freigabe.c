#include <stdio.h>
#include <stdlib.h>

int main()
{
  int * intwert = (int*) malloc(sizeof(int));
  double *doublewert = (double*) malloc(sizeof(double));
    printf("%lu",sizeof(int));
    printf("\n");
    printf("%lu",sizeof(double));
    printf("\n");
    printf("%lu",sizeof(intwert));
    printf("\n");
    printf("%lu",sizeof(doublewert));
    free(intwert);
    free(doublewert);
}