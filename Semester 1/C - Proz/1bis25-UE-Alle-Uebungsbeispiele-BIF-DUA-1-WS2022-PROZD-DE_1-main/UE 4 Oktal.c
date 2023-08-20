#include <stdio.h>

int main() {
    
    int oktal = 0;
    int dez = 0;
    int zsumme = 0;
    
    scanf("%3d" , &oktal );
    // 83= 3+16+64 
    dez=(oktal%10*1)+(oktal%100/10*8)+(oktal%1000/100*64);
    // 11= 3+8+0,83(keine kommas)
    zsumme=(dez%10)+(dez%100/10)+(dez/100);
    printf(": dec.: %d\n", dez);
    printf("ZS: %d", zsumme);
    return 0;
}