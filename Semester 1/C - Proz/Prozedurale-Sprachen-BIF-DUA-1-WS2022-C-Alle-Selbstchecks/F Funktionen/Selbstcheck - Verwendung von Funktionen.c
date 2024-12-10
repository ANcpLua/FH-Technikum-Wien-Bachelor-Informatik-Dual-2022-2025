#include <stdio.h>
#include <math.h>

int power (int x,int y) {
    int result=1;
    for(int i=0; i<y; i++) {
        result *= x;
    }
    return result;
    
}
int faculty(int x)
{
  int result=1;
  for(int i=1; i<=x; i++)
  {
    result = result*i;
  }
  return result;
}


int main()
{
  int x, y;
  char operator;
  scanf("%d",&x);
  scanf("%d",&y);
  scanf(" %c",&operator);

  if(operator =='a')
  {
    printf("%d",power(x,y));
  }
  if(operator =='b')
  {
    printf("%d",(faculty(x)+faculty(y)));
  }
  if(operator =='c')
  {
    printf("%d",faculty(power(x,y)));
  }

}