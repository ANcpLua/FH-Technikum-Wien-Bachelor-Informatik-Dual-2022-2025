#pragma once

#include <string>
#include <vector>

#include "charakter.h"
#include "faehigkeit.h"

class CharacterBuilder
        {
public:
    static Charakter erstelleCharakter(const std::string &name, const std::string &element, const std::vector<Faehigkeit> &charakterFaehigkeiten);
};
