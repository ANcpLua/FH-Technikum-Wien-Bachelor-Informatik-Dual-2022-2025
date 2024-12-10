#include <stdio.h>





int main()
{   
    int punkte = 0;
    scanf("%d", &punkte);
    
    if (punkte >= 0 && punkte <=100)
    {
        printf("Gültige Punktezahl");
    
    if (punkte >= 50)
    
        printf("\nPositive Note");
        
        else 
        
        printf("\nNegative Note");
    }   
        else if (punkte <= 0)
    {
        printf("Ungültige Punktezahl");
    }
        

    
    
    
    return 0;
