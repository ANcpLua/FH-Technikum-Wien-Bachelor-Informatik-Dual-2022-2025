#include "character_builder.h"

Charakter CharacterBuilder::erstelleCharakter(const std::string &name, const std::string &element, const std::vector<Faehigkeit> &charakterFaehigkeiten)
{
    return Charakter(name, 1000, element, charakterFaehigkeiten);
}
