#ifndef WORLD_H
#define WORLD_H

#include <iostream>
#include <vector>

#include "Field.h"
#include "PlainField.h"
#include "RecurringField.h"

class World
{
    public:
        World();
        virtual ~World();
        void printWorld();
        bool isEmpty();
        void executeField(int x, int y);

        const int WIDTH = 3;
        const int HEIGHT = 3;

    protected:

    private:
        std::vector< std::vector<Field*> > world;
};

#endif // WORLD_H
