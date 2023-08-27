#include <iostream>
#include <stdlib.h>
#include <time.h>
#include "World.h"

int main()
{
    srand (time(NULL));     //Seeding the rand function
    World world = World();  //Creating the game world

    while(!world.isEmpty()) //Harvest the world until it is empty
    {
        //Print the world and then call the execute-function of a random field
        world.printWorld();
        world.executeField(rand()%4, rand()%3); //CAUTION: The x coordinate can be invalid
    }

    world.printWorld(); //Print the world again in the end

    return 0;
}
