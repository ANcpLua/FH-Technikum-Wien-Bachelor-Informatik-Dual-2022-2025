#include <stdio.h>

int array[5];
float count = 0.00, sum = 0.00, average = 0.00;


int main()
{


    for(int i=0; i < 5; i++)
    {
        
        scanf("%d", &array[i]);

        if(array[i] == 0)
        {
            break;
        }
        count++;
        sum += array[i];
    

    }
    average = sum / count;
    printf("%.2lf\n", average);
    
    return 0;
}
