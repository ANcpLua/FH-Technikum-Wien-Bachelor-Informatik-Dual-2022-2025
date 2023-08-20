#include <stdio.h>

int is_prime(int x) 
{ 

    
    for(int i = 2; i< x; i++)
    {
        if(x % i == 0)
        {
       
                return 0;
        }
    }
    return 1;
    
}                
   

int main()
{
  int x=2,count=0;
  while(count<1010)
  {
    if(is_prime(x))
    {
      if (count>999)
      {
        printf("%d\n",x);
      }
      count++;
    }
    
    x++;
  }
}