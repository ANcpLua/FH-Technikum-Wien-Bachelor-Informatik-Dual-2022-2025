#include "IPlayer.h"

IPlayer::IPlayer(std::string name)
{
    this->name = name;
}

IPlayer::~IPlayer(){}

int IPlayer::getPoints()
{
    return points;
}

int IPlayer::getAttack()
{
    return attackPoints;
}

int IPlayer::getDefense()
{
    return defensePoints;
}

std::string IPlayer::getName()
{
    return name;
}

void IPlayer::rest()
{
    stamina = 100;
    attackBonus = 0;
}
