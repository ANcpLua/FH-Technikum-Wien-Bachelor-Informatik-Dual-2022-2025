#include "Spiel.h"
#include "Jaeger.h"
#include "Zerstoerer.h"
#include "Kreuzer.h"
#include <iostream>
#include <windows.h>
#include <limits>
#include "Schiff.h"


Spiel::Spiel() : flotte1("Flotte 1"), flotte2("Flotte 2") {}

void Spiel::start() {
    std::cout << "Spieler 1: Flotte erstellen\n";
    erstelleFlotte(flotte1);
    std::cout << "Flotte 1 erstellt!\n\n";
    std::cout << "Spieler 2: Flotte erstellen\n";
    erstelleFlotte(flotte2);
    std::cout << "Flotte 2 erstellt!\n\n";

    while (!flotte1.istZerstoert() && !flotte2.istZerstoert()) {
        spielerRunde(flotte1, flotte2);
        spielerRunde(flotte2, flotte1);
    }

    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleTextAttribute(hConsole, 6); // set color to yellow

    if (flotte1.istZerstoert()) {
        std::cout << "\nSpieler 2 hat gewonnen!" << std::endl;
    } else {
        std::cout << "\nSpieler 1 hat gewonnen!" << std::endl;
    }

    SetConsoleTextAttribute(hConsole, 7); // set color back to default
}

Schiff* Spiel::erzeugeSchiff() {
    int schiffTypIndex = validiereEingabe(1, 3,"Waehle den Typ des Schiffs 1: Jaeger, 2: Zerstoerer, 3: Kreuzer \n");

    switch (schiffTypIndex) {
        case 1:
            return new Jaeger();
        case 2:
            return new Zerstoerer();
        case 3:
            return new Kreuzer();
        default:
            throw std::runtime_error("Ungueltiger Schiffstyp");
    }
}

int Spiel::validiereEingabe(int min, int max, const std::string &prompt) {
    int input;
    while (true) {
        std::cout << prompt;
        std::cin >> input;
        if (std::cin.fail()) {
            std::cin.clear();
            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
            std::cout << "Ungültige Eingabe, bitte geben Sie eine Zahl ein.\n";
        } else if (input < min || input > max) {
            std::cout << "Ungueltige Eingabe. Bitte waehlen Sie zwischen " << min << " und " << max << ".\n";
        } else {
            return input;
        }
    }
}

void Spiel::erstelleFlotte(Flotte &flotte) {
    int schiffAnzahl = validiereEingabe(MIN_SHIPS, MAX_SHIPS,"Waehle die Anzahl der Schiffe (mindestens " + std::to_string(MIN_SHIPS) +", maximal " + std::to_string(MAX_SHIPS) + "):\n");
    for (int i = 0; i < schiffAnzahl; ++i) {
        Schiff *neuesSchiff = erzeugeSchiff();
        flotte.hinzufuegenSchiff(neuesSchiff);
    }
}

bool Spiel::angriffDurchfuehren(Schiff *angreifer, Schiff *verteidiger) {

    bool zerstoert = angreifer->spezialAngriff(*verteidiger);

    if (verteidiger->huellenwert <= 0) {
        HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleTextAttribute(hConsole, 4); // set color to yellow
        std::cout << verteidiger->name << " wurde zerstoert!\n";
        SetConsoleTextAttribute(hConsole, 7); // set color back to default
        return true;
    }

    return false;
}

void Spiel::angreifenMitFlotte(Flotte &angreifendeFlotte, Flotte &verteidigendeFlotte) {
    for (int i = 0; i < angreifendeFlotte.schiffe.size(); ++i) {
        Schiff *angreifer = angreifendeFlotte.schiffe[i];
        Schiff *verteidiger = verteidigendeFlotte.getSchiff(rand() % verteidigendeFlotte.schiffe.size());

        if (angriffDurchfuehren(angreifer, verteidiger)) {
            verteidigendeFlotte.entfernenSchiff(verteidiger);
        }
    }
}


void Spiel::spielerRunde(Flotte &angreifendeFlotte, Flotte &verteidigendeFlotte) {
    if (verteidigendeFlotte.istZerstoert()) {
        return;
    }
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleTextAttribute(hConsole, 2); // set color to yellow
    std::cout << "\nDie " << angreifendeFlotte.name << " greift an." << std::endl;
    SetConsoleTextAttribute(hConsole, 7); // set color back to default
    angreifenMitFlotte(angreifendeFlotte, verteidigendeFlotte);
}
