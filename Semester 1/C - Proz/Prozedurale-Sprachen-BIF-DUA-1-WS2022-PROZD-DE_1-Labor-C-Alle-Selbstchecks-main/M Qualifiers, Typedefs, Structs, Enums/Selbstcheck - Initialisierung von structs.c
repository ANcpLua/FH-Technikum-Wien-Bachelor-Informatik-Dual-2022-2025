#include <stdio.h>
#include <stdlib.h>

struct Punkt{
    int x,y;
};



int main()
{
    struct Punkt Membern;
    scanf("%d %d", &Membern.x, &Membern.y);
    printf("%d", Membern.x-Membern.y);
    return 0 ;
}
