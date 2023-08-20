#pragma once

#include "roboFabrik.h"
#include <vector>
#include <iostream>


class roboFabrik;
class World {
public:
    World();
    void PrintWorld(const std::vector<roboFabrik*>& robots);
    void setGrid(int x, int y, int z, int value);
    int getGrid(int x, int y, int z);
    bool nothingToMine() const;
    int getTotalScore();

private:
    using grid = std::vector<std::vector<std::vector<int>>>;
    grid Grid;
};
