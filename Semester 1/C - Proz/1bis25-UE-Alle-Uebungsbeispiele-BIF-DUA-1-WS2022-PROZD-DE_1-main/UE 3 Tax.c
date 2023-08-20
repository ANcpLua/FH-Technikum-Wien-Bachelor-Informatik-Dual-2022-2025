#include <stdio.h>
#include <stdlib.h>

int main()
{
    float brutto;
    float netto=1.2;
    
    scanf("%f", &brutto);
    scanf("%f", &netto);
    printf(": With tax: %.2f\n", brutto*netto);
    return 0;


}
