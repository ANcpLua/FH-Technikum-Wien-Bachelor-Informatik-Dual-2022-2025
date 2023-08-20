#include "Team.h"

void Team::hinzufuegenCharakter(const Charakter& charakter)
{
    charaktere.push_back(charakter);
}

void Team::leeren()
{
    charaktere.clear();
}

const std::vector<Charakter>& Team::getCharaktere() const
{
    return charaktere;
}
