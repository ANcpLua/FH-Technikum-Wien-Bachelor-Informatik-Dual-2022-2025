#include "World.h"
#include <random>
#include <algorithm>
#include <chrono>
#include <windows.h>
#include <iomanip>

using namespace std;

World::World() {
    Grid = grid();
    unsigned seed = std::chrono::high_resolution_clock::now().time_since_epoch().count();
    std::mt19937 generator(seed);
    std::uniform_int_distribution<int> distribution(1, 9);

    for (int x = 0; x < 5; x++) {
        Grid.emplace_back();
        for (int y = 0; y < 5; y++) {
            Grid.at(x).emplace_back(10);
            for (int z = 0; z < 10; z++) {
                Grid.at(x).at(y).at(z) = distribution(generator);
            }
        }
    }
}

void SetColor(int textcolor, int bgcolor) {
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleTextAttribute(hConsole, textcolor | bgcolor << 2);
}

void World::PrintWorld(const std::vector<roboFabrik*>& robots) {
    for (int row = 0; row < 2; row++) {
        for (int z = 0; z < 5; z++) {
            int currentLayer = z + row * 5;
            SetColor(15, 0);
            std::cout << "Layer " << currentLayer << ":           ";
        }
        std::cout << std::endl;

        for (int x = 0; x < 5; x++) {
            for (int z = 0; z < 5; z++) {
                int currentLayer = z + row * 5;
                for (int y = 0; y < 5; y++) {
                    auto it = std::find_if(robots.begin(), robots.end(), [&](roboFabrik* r) {
                        return r->isAlive() && x == r->getX() && y == r->getY() && currentLayer == r->getZ();
                    });

                    if (it != robots.end()) {
                        SetColor(4, 0);
                        std::cout << std::setw(2) << "R" << std::distance(robots.begin(), it) << " ";
                    } else {
                        int fieldValue = Grid[x][y][currentLayer];
                        SetColor(fieldValue == -10 ? 9 : 11, 0);
                        std::cout << std::setw(2) << (fieldValue == -10 ? " X " : std::to_string(fieldValue)) << " ";
                    }
                }
                std::cout << "   ";
                SetColor(15, 0);
            }
            std::cout << std::endl;
        }
    }
}


void World::setGrid(int x, int y, int z, int value) {
    Grid[x][y][z] = value;
}

int World::getGrid(int x, int y, int z) {
    return Grid[x][y][z];
}

int World::getTotalScore() {
    int totalScore = 0;
    for (auto& x : Grid) {
        for (auto& y : x) {
            for (auto& z : y) {
                int fieldValue = z;
                totalScore += (fieldValue != -10 ? fieldValue : 0);
            }
        }
    }
    return totalScore;
}


bool World::nothingToMine() const {
    for (int x = 0; x < 5; x++) {
        for (int y = 0; y < 5; y++) {
            bool allLayersMined = true;
            for (int z = 0; z < 10; z++) {
                if (Grid[x][y][z] != -10) {
                    allLayersMined = false;
                    break;
                }
            }
            if (!allLayersMined) {
                return false;
            }
        }
    }
    return true;
}
