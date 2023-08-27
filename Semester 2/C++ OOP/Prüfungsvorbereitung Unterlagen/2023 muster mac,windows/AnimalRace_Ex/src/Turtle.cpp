#include "Turtle.h"

Turtle::Turtle(std::string name, int distance) : Animal(name, distance)
{
    //ctor
}

Turtle::~Turtle()
{
    //dtor
}

void Turtle::run(){
    if(this->confused){
        if(rand() % 3 != 0){
            this->confused = false;
        }
        return;
    }

    distance -= 1;
}
