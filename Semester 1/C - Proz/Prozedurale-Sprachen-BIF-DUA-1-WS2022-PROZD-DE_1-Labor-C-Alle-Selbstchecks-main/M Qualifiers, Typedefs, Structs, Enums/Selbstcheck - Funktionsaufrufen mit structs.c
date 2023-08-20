#include <stdio.h>
#include <stdlib.h>

struct Punkte{
    int x;
    int y;
};

void vertauschen(struct Punkte* Membern)
{
    int z = Membern->x;
    Membern->x = Membern->y;
    Membern->y=z;

}

int main()
{
    struct Punkte Membern;
    scanf("%d %d", &Membern.x, &Membern.y);
    printf("%d\n", Membern.x-Membern.y);
    vertauschen(&Membern);
    printf("%d\n", Membern.x-Membern.y);
    return 0 ;
}
