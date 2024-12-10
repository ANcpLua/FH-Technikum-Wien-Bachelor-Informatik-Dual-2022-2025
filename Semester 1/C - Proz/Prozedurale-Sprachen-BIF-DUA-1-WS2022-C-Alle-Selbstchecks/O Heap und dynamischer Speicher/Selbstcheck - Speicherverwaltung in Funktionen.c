#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void check(char *p)
{
    if (p == NULL)
    exit(1);
}

int sende(char ar[])
{
    int len=0;
    for(int i=0;;i++)
    {
        if(ar[i]== '\0')
        {
            len=i;
            break;
        }
    }
    return len;
}

char* str(char ar[], char* s) 
{ 
    int len = sende(ar); 
    s = (char*)realloc(s,(len+1)*sizeof(char)); 
    check(s);
    for(int i=0; i<(len/2); ++i) 
    {   
    
        s[i]= ar[i];
        s[len-1-i]=ar[i];
    } 
    if (len%2) {
    
        s[len/2]=ar[len/2];}
        

    return s; 
}

int main()
{   
    
    char* s = (char*)malloc(sizeof(char));
    check(s);
    char ar[111]="";
    scanf("%s", ar);
    s = str(ar,s);
    printf("%s\n", s);
   
    free(s);
}
