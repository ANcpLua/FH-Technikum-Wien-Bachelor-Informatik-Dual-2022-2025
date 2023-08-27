#ifndef RACE_H
#define RACE_H
#include "Animal.h"

class Race
{
    public:
        Race();
        virtual ~Race();
        void competition();
        void addAnimal(Animal* animal);
        bool raceFinished();
        void printResult();
        void trickEachother();

    protected:
        std::vector<Animal*> animals;

};

#endif // RACE_H
