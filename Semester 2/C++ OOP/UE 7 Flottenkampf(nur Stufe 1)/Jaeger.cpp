#include "Jaeger.h"
#include <iostream>
#include <windows.h>

Jaeger::Jaeger() : Schiff("Jaeger", 75, 4, 30) {}

bool Jaeger::spezialAngriff(Schiff& other) {
    bool hit = Schiff::spezialAngriff(other);
    if (hit && (rand() % 10) + 1 >= 9) {  // Bei 9 oder 10 wird doppelter Schaden verursacht
        other.huellenwert -= this->schaden;
        HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleTextAttribute(hConsole, 5); // set color to yellow
        std::cout << "Kritischer Treffer! Zusaetzlicher Schaden von " << this->schaden << " Punkten!\n";
        SetConsoleTextAttribute(hConsole, 7); // set color back to default
    }
    return hit;
}
