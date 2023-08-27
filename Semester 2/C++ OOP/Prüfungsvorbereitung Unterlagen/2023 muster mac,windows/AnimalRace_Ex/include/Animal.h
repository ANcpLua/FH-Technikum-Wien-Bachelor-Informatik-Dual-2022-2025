#ifndef ANIMAL_H
#define ANIMAL_H
#include <iostream>
#include <string>
#include <vector>

class Animal
{
    public:
        Animal(std::string name, int distance);
        virtual ~Animal();
        virtual void run() = 0;
        bool atGoal();
        void trick(Animal* opponent);
        std::string getName() const;
        int getDistance() const;

    protected:
        std::string name;
        int distance;
        bool confused;

    private:
};

#endif // ANIMAL_H
