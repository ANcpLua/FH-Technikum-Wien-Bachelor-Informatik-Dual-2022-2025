#include "Kreuzer.h"
#include <iostream>
#include <windows.h>

Kreuzer::Kreuzer() : Schiff("Kreuzer", 250, 8, 50) {}

bool Kreuzer::spezialAngriff(Schiff& other) {
    int attackCount = 0;
    int prevHuellenwert = other.huellenwert;

        while (true) {
            Schiff::spezialAngriff(other);
            ++attackCount;

            if (other.huellenwert == prevHuellenwert || other.istZerstoert()) {
                    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
                    SetConsoleTextAttribute(hConsole, 1); // set color to yellow
                    std::cout << attackCount << " Angriff" << (attackCount == 1 ? " wurde" : "e wurden") << " durchgefuehrt.\n";
                    SetConsoleTextAttribute(hConsole, 7); // set color back to default
                    break;
            }

            prevHuellenwert = other.huellenwert;
        }
        return false;
    }
