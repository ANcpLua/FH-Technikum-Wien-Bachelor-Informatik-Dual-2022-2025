#include <iostream>
#include <stdlib.h>
#include <time.h>
#include "Floor.h"

int main()
{
    srand (time(NULL));     //Seeding the rand function
    Floor floor = Floor();  //Creating the floor

    while(!floor.isRed())   //Mark and Replace until the whole floor is red
    {
        //Print the floor and then call the mark- and replace-function of a random tile
        floor.print();
        floor.mark(rand()%4, rand()%3); //CAUTION: The x coordinate can be invalid
        floor.replace(rand()%3, rand()%4); //CAUTION: The y coordinate can be invalid
    }

    floor.print(); //Print the floor again in the end

    return 0;
}
