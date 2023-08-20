#include "Charakter.h"

Charakter::Charakter(std::string name, int lebenspunkte, std::string element, const std::vector<Faehigkeit>& faehigkeiten)
        : name(name), lebenspunkte(lebenspunkte), element(element), faehigkeiten(faehigkeiten)
{
}

bool Charakter::istTot() const
{
    return lebenspunkte <= 0;
}
