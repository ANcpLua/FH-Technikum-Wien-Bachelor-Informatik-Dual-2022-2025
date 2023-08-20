#pragma once

#include "roboFabrik.h"
#include "World.h"

class roboter : public roboFabrik{
public:
    roboter(int x, int y, int z);
    int mine(World& world, int dx, int dy, int dz, const std::vector<roboFabrik*>& robots) override;
};

