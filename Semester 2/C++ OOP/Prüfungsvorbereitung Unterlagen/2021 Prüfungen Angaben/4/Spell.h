#ifndef SPELL_H
#define SPELL_H

#include "Wizard.h"

class Wizard;

class Spell
{
    public:
        Spell(int type);
        virtual ~Spell();
        int getType();

    protected:
        int type; 	//Instants are either damaging or healing
					//Sorceries are either debuffing or buffing

    private:
};

#endif // SPELL_H
