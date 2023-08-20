#ifndef CHARAKTER_H
#define CHARAKTER_H

#include <string>
#include <vector>
#include "Faehigkeit.h"

class Charakter
{
public:
    std::string name;
    int lebenspunkte;
    std::string element;
    std::vector<Faehigkeit> faehigkeiten;

    Charakter(std::string name, int lebenspunkte, std::string element, const std::vector<Faehigkeit>& faehigkeiten);

    bool istTot() const;
};

#endif // CHARAKTER_H
