#include "Arena.h"
#include <iostream.h>

Arena::attackMonster(int monsterNumber, Creature& attacker){


    int attackerdamage = attacker.getDamage();

    /*attacked monster*/->dealDamage(attackerdamage);

    std::cout << "Hero deals " << attackerdamage << " damage to " << /*attacked monster*/->getName();

    if(/*attacked monster*/->isAlive()){

        int monsterdamage = /*attacked monster*/->getDamage();

        attacker.dealDamage(monsterdamage);

        std::cout << " and it defends itself with " << monsterdamage << " damage." << std::endl;

    }
    else{

        std::cout << " and defeats it!" << std::endl;

        std::cout << "There are " << getMonsterAmount() << " Monsters remaining." << std::endl;

    }

}
