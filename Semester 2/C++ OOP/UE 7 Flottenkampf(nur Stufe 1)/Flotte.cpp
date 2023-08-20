#include "Flotte.h"
#include <algorithm>
#include <stdexcept>

Flotte::Flotte(std::string name): name(name) {}

void Flotte::hinzufuegenSchiff(Schiff* schiff) {
    schiffe.push_back(schiff);
}

void Flotte::entfernenSchiff(Schiff* schiff) {
    schiff->zerstoert = true;
    schiffe.erase(std::remove(schiffe.begin(), schiffe.end(), schiff), schiffe.end());
    delete schiff;
}

bool Flotte::istZerstoert() const {
    return schiffe.empty();
}

Schiff* Flotte::getSchiff(int index) {
    if (index < 0 || index >= schiffe.size())
        throw std::out_of_range("Schiffindex ist ungueltig!");
    return schiffe[index];
}

Flotte::~Flotte() {
}
