//
//  PlayerA.hpp
//  OOP_EX
//
//  Created by John Kulha on 09.06.22.
//

#ifndef PlayerA_hpp
#define PlayerA_hpp

#include <stdio.h>
#include "IPlayer.h"

class PlayerA : public IPlayer {
    public:
        PlayerA(std::string name);
    
        virtual bool attack(IPlayer* enemy);
        virtual double beAttacked();
};

#endif /* PlayerA_hpp */
