#include "Zerstoerer.h"
#include <algorithm>

Zerstoerer::Zerstoerer() : Schiff("Zerstoerer", 100, 4, 25) {}

bool Zerstoerer::spezialAngriff(Schiff &other) {
    int wurf = (rand() % 10) + 1;  // Würfelwurf zwischen 1 und 10
    int minWurf = std::max(1, other.groesse - 2);  // Mindestens 1, maximal Groesse-2
    if (wurf >= minWurf) {
        Schiff::spezialAngriff(other);
    }
    return false;
}
