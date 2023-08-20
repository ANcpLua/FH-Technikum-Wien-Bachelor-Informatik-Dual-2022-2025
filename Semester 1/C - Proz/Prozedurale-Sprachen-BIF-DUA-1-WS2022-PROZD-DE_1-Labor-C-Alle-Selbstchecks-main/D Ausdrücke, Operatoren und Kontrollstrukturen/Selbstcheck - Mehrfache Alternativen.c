#include <stdio.h>





int main()
{
    int punkte = 0;
    scanf("%d", &punkte);

    if(punkte > 90 && punkte <= 100)
    
        printf("Sehr gut\n");
    else if(punkte > 76 && punkte <= 90)
        printf("Gut\n");
    else if(punkte > 63 && punkte <= 76)
        printf("Befriedigend\n");
    else if(punkte >= 50 && punkte <= 63)
        printf("Genuegend\n");
    else if(punkte >= 0 && punkte < 50)
        printf("Nicht genuegend\n");
    else
        printf("Ungueltige Punktezahl\n");
    


}