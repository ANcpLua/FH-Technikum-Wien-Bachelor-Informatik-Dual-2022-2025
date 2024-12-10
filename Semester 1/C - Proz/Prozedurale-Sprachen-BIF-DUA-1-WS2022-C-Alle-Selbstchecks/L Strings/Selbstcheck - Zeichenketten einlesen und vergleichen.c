#include <stdio.h>
#include <string.h>

int main()
{
    char h[6], w[6];
    int irgendwas = 0;
    fgets(h, 6, stdin);
    fgets(w, 6, stdin);
    irgendwas = strcmp(h, w);
    
    if(irgendwas)
    {
        printf("Zeichenkette ungleich");
        
    }       
        else 
        {
            printf("Zeichenkette gleich");
        }
    
    
    
}