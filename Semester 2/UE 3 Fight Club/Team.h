#ifndef TEAM_H
#define TEAM_H

#include "Charakter.h"
#include <vector>

class Team
{
public:
    void hinzufuegenCharakter(const Charakter& charakter);
    void leeren();
    const std::vector<Charakter>& getCharaktere() const;
    std::vector<Charakter> charaktere;
};

#endif
