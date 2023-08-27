#include "Spell.h"

Spell::Spell(int type)
{
    this->type = type;
}

Spell::~Spell(){}

int Spell::getType()
{
    return type;
}
