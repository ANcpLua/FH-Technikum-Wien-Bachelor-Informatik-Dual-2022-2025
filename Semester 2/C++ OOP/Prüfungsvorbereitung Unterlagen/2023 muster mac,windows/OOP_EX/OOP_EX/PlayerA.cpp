//
//  PlayerA.cpp
//  OOP_EX
//
//  Created by John Kulha on 09.06.22.
//

#include "PlayerA.hpp"

PlayerA::PlayerA(std::string name) : IPlayer(name) {
    attackBonus = 0;
    attackPoints = 40;
    defensePoints = 60;
    points = 0;
    stamina = 100;
}

bool PlayerA::attack(IPlayer *enemy) {
    if (enemy == NULL)
        throw new std::invalid_argument("Enemy is null");
    
    attackBonus += rand() % 5;
    double tmpAttackValue = (attackBonus + attackPoints) * enemy->beAttacked();
    
    if (tmpAttackValue > enemy->getDefense()) {
        points++;
        return true;
    }
    
    return false;
}

double PlayerA::beAttacked() {
    stamina -= 3;
    if (stamina < 0)
        stamina = 0;
    
    if (stamina % 7 == 0)
        return 5;
    if (stamina % 5 == 0)
        return 3;
    if (stamina % 2 == 0)
        return 1;
    
    return 0;
}
