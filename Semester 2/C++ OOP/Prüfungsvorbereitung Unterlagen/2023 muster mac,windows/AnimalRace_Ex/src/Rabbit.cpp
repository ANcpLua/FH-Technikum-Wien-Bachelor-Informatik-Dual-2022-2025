#include "Rabbit.h"

Rabbit::Rabbit(std::string name, int distance) : Animal(name, distance)
{
    this->asleep = asleep;
}

Rabbit::~Rabbit()
{
    //dtor
}

void Rabbit::run(){
    if(this->asleep){
        if(rand() % 3 == 0){
            this->asleep = false;
        }
        return;
    }
    if(this->confused){
        if(rand() % 3 == 0){
            this->confused = false;
        }
        return;
    }

    distance -= 3;

    if(!this->atGoal()){
        if(rand() % 3 == 0){
            this->asleep = true;
            std::cout << "The Rabbit fell asleep " << this->distance << " meters from the goal" << std::endl;
        }
    }
}
