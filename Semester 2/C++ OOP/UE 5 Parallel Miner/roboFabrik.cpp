#include "roboFabrik.h"

using namespace std;

roboFabrik::roboFabrik(int x, int y, int z, int health) : posX(x), posY(y), posZ(z), health(health) {}

int roboFabrik::getX() const { return posX; }

int roboFabrik::getY() const { return posY; }

int roboFabrik::getZ() const { return posZ; }

void roboFabrik::setPosition(int x, int y, int z) { posX = x, posY = y, posZ = z; }

void roboFabrik::attack(roboFabrik* other) {
    if (other) {
        cout << "Roboter bei (" << getX() << ", " << getY() << ", " << getZ() << ") greift Roboter bei ("
             << other->getX() << ", " << other->getY() << ", " << other->getZ() << ") an.\n";
        other->takeDamage(0);
    }
}

void roboFabrik::takeDamage(int damage) {
    damage = rand() % 10 + 1;
    health -= damage;
    cout << "Roboter bei (" << getX() << ", " << getY() << ", " << getZ() << ") nimmt " << damage << " Schaden. "
         << "Verbleibende HP: " << health << ".\n"
         << (health <= 0 ? "Roboter wurde zerstoert.\n\n" : "\n");
}
