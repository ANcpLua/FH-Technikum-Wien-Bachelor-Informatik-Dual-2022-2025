#include <stdio.h>
int main()
{
  int arr[10];
  // einlesen
  for(int i = 0; i < 10; i++)
  {
      scanf("%d", &arr [i]);
  }
  // verschieben
  for (int i= 9; i > 0; i--)
  {
      arr[i]= arr[i-1];
  }
  arr[0] = 0; 
  // print 
  for (int i =0; i < 10; i++)
  {
      printf("%d", arr[i]);
  }
  return 0;
}