#include <iostream>

using namespace std;

int main()
{
    cout << "Hello MonsterSlayer!" << endl;

    //Create your objects

    cout << /*Hero*/.getName() << " enters an arena with " << /*Arena*/.getMonsterAmount() << " monsters." << endl;

    while(/*Hero*/.isAlive() && /*Arena*/.getMonsterAmount() > 0){
        /*Arena*/.attackMonster(rand()%15, /*Hero*/);
    }

    if(/*Hero*/.isAlive()){
        cout << /*Hero*/.getName() << " has defeated all the monsters!" << /*Hero*/.getHP() << endl;
    }
    else{
        cout << /*Hero*/.getName() << " was defeated by one of the " << /*Arena*/.getMonsterAmount() << " remaining monsters..." << endl;
    }

    return 0;
}
