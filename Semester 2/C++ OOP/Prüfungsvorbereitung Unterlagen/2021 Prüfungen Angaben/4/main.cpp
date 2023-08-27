#include <iostream>
#include <stdlib.h>
#include <time.h>
#include "Wizard.h"
#include "Spell.h"
#include "Instant.h"
#include "Sorcery.h"

int main()
{
    srand (time(NULL)); //Initializing random seed

    Wizard wizOne = Wizard("Xardas"); //Creating the wizards
    Wizard wizTwo = Wizard("Pyrokar");

    while(wizOne.spellsLeft() || wizTwo.spellsLeft())
    {//Casting spells until no spells are left
        if(wizOne.spellsLeft())
            wizOne.castSpell(&wizTwo);
        if(wizTwo.spellsLeft())
            wizTwo.castSpell(&wizOne);
    }

    wizOne.print(); //Print final state
    wizTwo.print();

    return 0;
}
