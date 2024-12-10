#include <stdio.h>

int main()
{

  int zweidimensionales[3][3];   // a b c ; d e f 
  for(int a=0;a<3;a++)           // a=0 0+1+1
  {
      
    for(int b=0;b<3;b++)            // b=0 0+1+1
    {
      zweidimensionales[a][b]=a+b;
      printf("%d\n",zweidimensionales[a][b]);

    }
  }
}
