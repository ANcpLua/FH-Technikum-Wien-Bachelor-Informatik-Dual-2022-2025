#include <iostream>
#include <stdlib.h>
#include <time.h>
#include <vector>
#include "IPlayer.h"
#include "PlayerA.hpp"
#include "PlayerB.hpp"

using namespace std;

int main()
{
    //Seeding rand
    srand(time(NULL));

    //Initializing players
    IPlayer* p1 = new PlayerA("Peco");
    IPlayer* p2 = new PlayerB("Smile");
    //IPlayer* p3 = new PlayerA("Hi");
    
    /*std::vector<IPlayer*> players;
    players.push_back(p1);
    players.push_back(p2);
    players.push_back(p3);*/
    
    // Kann nicht aufgerufen werden, da attack nicht implementiert sein kann!
    //IPlayer* p = new IPlayer("Hi");

    //Playing 9 Rounds
    int round = 1;
    while(round<10)
    {
        cout << "+++++ ROUND " << round << " +++++\n";

        //Playing until a point is scored
        while(true)
        {
            try {
            if(p1->attack(p2)) //Player one attacks player two
            {
                cout << p1->getName() << " scored.\n";
                break;
            }
            else //If no point was scored, player two attacks player one
                if(p2->attack(p1))
                {
                    cout << p2->getName() << " scored.\n";
                    break;
                }
                
            } catch (std::invalid_argument& e) {
                cout << "One player is null" << endl;
            }
        }

        //Preparing next round by resting and changing players
        ++round;
        p1->rest();
        p2->rest();
        IPlayer* temp = p1;
        p1 = p2;
        p2 = temp;
        
        // Beispiel wie man Positionen tauschen kann mit vectoren
        //players.push_back(players.front());
        //players.erase(players.begin());
    }

    //Printing result
    try {
        if(*p1>p2)
            cout << p1->getName() << " won with " << p1->getPoints() << " points.\n";
        else
            cout << p2->getName() << " won with " << p2->getPoints() << " points.\n";
    } catch (std::invalid_argument& e) {
        cout << "One player is null!" << endl;
    }

    //Cleaning up
    delete p1;
    delete p2;
    return 0;
}
