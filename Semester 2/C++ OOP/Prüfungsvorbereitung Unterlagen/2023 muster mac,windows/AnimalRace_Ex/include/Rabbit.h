#ifndef RABBIT_H
#define RABBIT_H
#include "Animal.h"

class Rabbit : public Animal
{
    public:
        Rabbit(std::string name, int distance);
        virtual ~Rabbit();
        void run() override;

    protected:
        bool asleep;

    private:
};

#endif // RABBIT_H
