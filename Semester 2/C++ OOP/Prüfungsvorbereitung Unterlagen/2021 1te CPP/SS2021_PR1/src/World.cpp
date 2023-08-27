#include "World.h"

World::World()
{
    std::cout << "[*] World Constructor" << std::endl;

    for(int y = 0; y < this->HEIGHT; ++y)
    {
        std::vector<Field*> tmp;

        for(int x = 0; x < this->WIDTH; ++x)
        {
            if(rand() % 2 == 0)
            {
                tmp.push_back(new PlainField());
            }

            else
            {
                tmp.push_back(new RecurringField());
            }
        }

        world.push_back(tmp);
    }
}

World::~World()
{
    std::cout << "[*] World Destructor" << std::endl;

    for(int y = 0; y < this->HEIGHT; ++y)
    {
        for(int x = 0; x < this->WIDTH; ++x)
        {
            if(world.at(y).at(x) != nullptr)
            {
                delete world.at(y).at(x);
                world.at(y).at(x) = nullptr;
            }
        }
    }
}

void World::printWorld()
{
    std::cout << "World:" << std::endl;

    for(int y = 0; y < this->HEIGHT; ++y)
    {
        for(int x = 0; x < this->WIDTH; ++x)
        {
            world.at(y).at(x)->displayField();
        }

        std::cout << std::endl;
    }

    std::cout << std::endl;
}

bool World::isEmpty()
{
    for(int y = 0; y < this->HEIGHT; ++y)
    {
        for(int x = 0; x < this->WIDTH; ++x)
        {
            if(world.at(y).at(x) != nullptr)
            {
                return false;
            }
        }
    }

    return true;
}

void World::executeField(int x, int y)
{
    if(!(x < this->WIDTH))
    {
        throw std::invalid_argument("[!] X-Coordinate is not in Range of Width");
    }

    if(!(y < this->HEIGHT))
    {
        throw std::invalid_argument("[!] Y-Coordinate is not in Range of Height");
    }

    if(world.at(y).at(x) != nullptr)
    {
        if(world.at(y).at(x)->execute())
        {
            if(world.at(y).at(x)->getPoints() > 0)
            {
                int randX;
                int randY;

                do
                {
                    randX = rand() % this->WIDTH;
                    randY = rand() % this->HEIGHT;
                }
                while(world.at(randY).at(randX)->getDisplay() != PlainField().getDisplay());

                delete world.at(randY).at(randX);
                world.at(randY).at(randX) = new RecurringField();

                delete world.at(y).at(x);
                world.at(y).at(x) = new PlainField();
            }
        }
    }
}
