#include <stdio.h>
int main()
{
  int array[100], c, d, position, t;

for (c = 0; c < 10; c++)
    scanf("%d", &array[c]);

  for (c = 0; c < (10 - 1); c++) // finding minimum element (n-1) times
  {
    position = c;

    for (d = c + 1; d < 10; d++)
    {
      if (array[position] > array[d])
        position = d;
    }
    if (position != c)
    {
      t = array[c];
      array[c] = array[position];
      array[position] = t;
    }
  }


  for (c = 0; c < 10; c++)
    printf("%d ", array[c]);

  return 0;
}