//
//  PlayerB.hpp
//  OOP_EX
//
//  Created by John Kulha on 09.06.22.
//

#ifndef PlayerB_hpp
#define PlayerB_hpp

#include "IPlayer.h"

class PlayerB : public IPlayer {
    public:
        PlayerB(std::string name);
    
        virtual bool attack(IPlayer* enemy);
        virtual double beAttacked();
    protected:
        bool confused;
};
#endif /* PlayerB_hpp */
