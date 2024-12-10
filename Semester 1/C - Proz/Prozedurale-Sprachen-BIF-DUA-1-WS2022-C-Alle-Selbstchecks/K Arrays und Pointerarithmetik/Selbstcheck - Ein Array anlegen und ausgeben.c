#include <stdio.h>

int main()
{
 int size;
 int startValue;
 scanf("%d%d", &size,&startValue);
 int arr[size];
 for(int n=0;n<size;++n)
  {
    arr[n]=n+startValue;
    printf("%d\n", arr[n]);
  }
     return 0;
}





