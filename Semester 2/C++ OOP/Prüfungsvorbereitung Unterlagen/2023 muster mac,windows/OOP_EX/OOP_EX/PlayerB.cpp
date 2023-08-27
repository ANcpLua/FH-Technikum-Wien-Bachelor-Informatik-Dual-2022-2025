//
//  PlayerB.cpp
//  OOP_EX
//
//  Created by John Kulha on 09.06.22.
//

#include "PlayerB.hpp"

PlayerB::PlayerB(std::string name) : IPlayer(name) {
    attackBonus = 0;
    attackPoints = 45;
    defensePoints = 55;
    points = 0;
    stamina = 100;
    confused = false;
}

bool PlayerB::attack(IPlayer* enemy) {
    if (enemy == NULL)
        throw new std::invalid_argument("Enemy is null");
    
    if ((rand() % 11 + 1) < 4)
        confused = !confused;
    
    if (!confused)
        attackBonus = 5 * enemy->beAttacked();
    
    int tmpAttackValue = attackPoints + attackBonus;
    
    if (tmpAttackValue > enemy->getDefense()) {
        points++;
        return true;
    }
    
    return false;
}

double PlayerB::beAttacked() {
    if (confused) {
        stamina -= stamina / 5;
    } else {
        stamina -= stamina / 20;
    }
    
    if (stamina > 30)
        return 0;
    
    return 1;
}
