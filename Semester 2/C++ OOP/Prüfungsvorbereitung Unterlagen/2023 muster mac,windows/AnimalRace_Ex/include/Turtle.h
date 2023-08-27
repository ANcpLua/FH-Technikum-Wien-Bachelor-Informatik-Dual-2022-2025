#ifndef TURTLE_H
#define TURTLE_H
#include "Animal.h"

class Turtle : public Animal
{
    public:
        Turtle(std::string name, int distance);
        virtual ~Turtle();
        void run() override;

    protected:

    private:
};

#endif // TURTLE_H
