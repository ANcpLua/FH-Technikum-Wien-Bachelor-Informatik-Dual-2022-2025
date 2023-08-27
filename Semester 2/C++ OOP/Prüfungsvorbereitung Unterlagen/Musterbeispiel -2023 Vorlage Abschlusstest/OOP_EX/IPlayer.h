#ifndef IPLAYER_H
#define IPLAYER_H

#include <iostream>
#include <stdlib.h>

class IPlayer
{
    public:
        IPlayer(std::string name);
        virtual ~IPlayer();
        void rest();
        int getPoints();
        int getAttack();
        int getDefense();
        std::string getName();

    protected:
        int points;
        int defensePoints;
        int attackPoints;
        int attackBonus;
        int stamina;
        std::string name;

    private:

};

#endif // IPLAYER_H
