#include <stdio.h>

int main() {
    
    int x=0;
    
    do 
    {
        printf("1. Continue\n");
        printf("2. Quit\n");
        printf("Your selection:\n");
        scanf("%d", &x);
        if(x==1) 
        {
        printf("Let's continue\n");
        }
        else if (x==2) 
        {
            printf("Bye, bye\n");
            break;
        }
        else if (x!=1 || x!=2)
        {
        printf("Unknown selection\n");
        }
        
        
    }
    while (1);
    
     
       
    
    
        
    
    return 0;
}