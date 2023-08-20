#include "roboter.h"
#include "roboFabrik.h"
#include <vector>
#include <algorithm>

using namespace std;

roboter::roboter(int x, int y, int z) : roboFabrik(x, y, z, 165) {}

int roboter::mine(World& world, int moveX, int moveY, int dz, const std::vector<roboFabrik*>& robots) {
    if (!isAlive()) return 0; // If the robot is not alive, do nothing

    // Compute the new position
    int posX = moveX;
    int posY = moveY;

    // Check if the new position is within the world boundaries
    if (posX >= 0 && posX < 5 && posY >= 0 && posY < 5) {
        // Check for other robots on the same or adjacent fields
        roboFabrik* other = nullptr;
        for (roboFabrik* robot : robots) {
            if (robot != this && robot->getX() == posX && robot->getY() == posY) {
                other = robot;
                break;
            }
        }
        if (other != nullptr && other->getHealth() > 0) {
            // Attack the other robot and allow it to counter-attack
            attack(other);
            other->attack(this);
        }

        // Update the robot's position
        setPosition(posX, posY, getZ());

        // Get the values of the fields in the robot's current position
        vector<int> fieldValues(10);
        for (int z = 0; z < 10; z++) {
            fieldValues[z] = world.getGrid(posX, posY, z);
        }

        // Randomly choose a resource to mine
        int minedValue = 0;
        if (health > 0) {
            vector<int> availableFields;
            for (int z = 0; z < 10; z++) {
                if (fieldValues[z] >= 0) {
                    availableFields.push_back(z);
                }
            }
            if (!availableFields.empty()) {
                int randomFieldIndex = rand() % availableFields.size();
                int minedZ = availableFields[randomFieldIndex];
                minedValue = fieldValues[minedZ];
                world.setGrid(posX, posY, minedZ, -10);
            }
        }

        return minedValue;
    }

    return 0;
}
