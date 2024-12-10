#include <stdio.h>
#include <string.h>

int main()
{   
    char s[40];
    scanf("%[^!]s", s);   // ^ ohne !
    printf("%zu\n", strlen(s));       // %ld,%lu geht auch, %zu wegen size_t mit nur %d gabs fehler laut google funkts so.
    printf("%s", s);
    return 0;
}
