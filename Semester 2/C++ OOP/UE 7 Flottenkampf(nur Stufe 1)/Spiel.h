#ifndef SPIEL_H
#define SPIEL_H

#include "Flotte.h"

class Spiel {
public:
    Spiel();
    void start();
    static const int MIN_SHIPS = 1;
    static const int MAX_SHIPS = 9;

private:
    Flotte flotte1;
    Flotte flotte2;
    Schiff *erzeugeSchiff();
    int validiereEingabe(int min, int max, const std::string &prompt);
    void erstelleFlotte(Flotte &flotte);
    bool angriffDurchfuehren(Schiff *angreifer, Schiff *verteidiger);
    void angreifenMitFlotte(Flotte &angreifendeFlotte, Flotte &verteidigendeFlotte);
    void spielerRunde(Flotte &angreifendeFlotte, Flotte &verteidigendeFlotte);
};

#endif //SPIEL_H
