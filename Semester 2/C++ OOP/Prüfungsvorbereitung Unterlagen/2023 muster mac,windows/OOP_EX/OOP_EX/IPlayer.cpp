#include "IPlayer.h"
#include <exception>

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

bool IPlayer::attack(IPlayer* enemy) {
    std::cout << "Attack from IPlayer" << std::endl;
    return true;
}

double IPlayer::beAttacked() {
    std::cout << "beAttacked from IPlayer" << std::endl;
    return 0;
}

bool IPlayer::operator>(IPlayer* enemy) {
    if (enemy == nullptr)
        throw new std::invalid_argument("Enemy is null");
    
    if (points > enemy->getPoints())
        return true;
    return false;
}
