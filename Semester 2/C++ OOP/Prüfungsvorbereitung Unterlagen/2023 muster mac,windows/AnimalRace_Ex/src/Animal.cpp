#include "Animal.h"

Animal::Animal(std::string name, int distance) : name(name), distance(distance)
{
    this->confused = false;
}

Animal::~Animal()
{
    //dtor
}

bool Animal::atGoal(){
    if(distance < 1){
        distance = 0;
        return true;
    } else {
        return false;
    }
}

void Animal::trick(Animal* opponent){
    if(opponent == nullptr){
        throw std::invalid_argument("Opponent NULL!");
    }
    if(std::abs(this->distance - opponent->distance) > 5){
        throw std::logic_error("Opponent too far away!");
    }

    if(rand() % 6 == 0){
        opponent->confused = true;
        std::cout << "The " << opponent->name << " got distracted" << std::endl;
    }
}

std::string Animal::getName() const{
    return name;
}

int Animal::getDistance() const{
    return distance;
}
