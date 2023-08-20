#ifndef COMPUTER_H
#define COMPUTER_H

#include <vector>
#include <string>
#include "Charakter.h"
#include "Faehigkeit.h"

class Computer
{
public:
    static Charakter erstelleZufaelligenCharakter(const std::vector<Faehigkeit>& verfuegbareFaehigkeiten, const std::vector<std::string>& verfuegbareElemente);
};

#endif // COMPUTER_H
