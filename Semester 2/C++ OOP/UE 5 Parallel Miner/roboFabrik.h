#pragma once

#include <vector>
#include "World.h"


class World;
class roboFabrik {
public:
    roboFabrik(int x, int y, int z, int health);
    virtual ~roboFabrik() {}
    int getX() const;
    int getY() const;
    int getZ() const;
    int getHealth() const {return health;}
    bool isAlive() const {return health > 0;}
    virtual int mine(World& world, int dx, int dy, int dz, const std::vector<roboFabrik*>& robots) = 0;
    void setPosition(int x, int y, int z);
    void attack(roboFabrik* other);
    void takeDamage(int damage);

protected:
    int health; int posX; int posY; int posZ;
};
