#include <stdio.h>

int main() {
    int hour24 = 0, minutes = 0, hour12 = 0;
    printf("24h time: ");
    
    scanf("%2d:%2d", &hour24, &minutes);
    
    if(hour24 > 12 && hour24 < 24) {
        hour12 = hour24 - 12;
        printf("12h time: %2d:%02d PM", hour12,minutes);
    }
    else if(hour24 == 12){
        printf("12h time: %2d:%02d PM", hour24,minutes);
    }
    else if(hour24 == 0){
        hour12 = 12;
        printf("12h time: %2d:%02d AM", hour12,minutes); 
    }
    else if(hour24 < 12){
        printf("12h time: %2d:%02d AM", hour24,minutes);
    }
    return 0;
}

