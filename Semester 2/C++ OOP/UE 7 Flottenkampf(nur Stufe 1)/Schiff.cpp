#include "Schiff.h"
#include <iostream>

Schiff::Schiff(std::string name, int huellenwert, int groesse, int schaden, bool zerstoert)
        : name(std::move(name)), huellenwert(huellenwert), groesse(groesse), schaden(schaden), zerstoert(zerstoert) {}

bool Schiff::spezialAngriff(Schiff& other) {
    int wurf = (rand() % 10) + 1;
    if (wurf >= other.groesse) {
        other.huellenwert -= this->schaden;
        HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleTextAttribute(hConsole, 14); // set color to yellow
        std::cout << this->name << " hat " << other.name << " fuer " << this->schaden << " Punkte getroffen!\n";
        SetConsoleTextAttribute(hConsole, 7); // set color back to default
        return true;
    } else {
        std::cout << this->name << " hat " << other.name << " verfehlt!\n";
        return false;
    }
}

bool Schiff::istZerstoert() const {
    return huellenwert <= 0 || zerstoert;
}

Schiff::~Schiff() {}
