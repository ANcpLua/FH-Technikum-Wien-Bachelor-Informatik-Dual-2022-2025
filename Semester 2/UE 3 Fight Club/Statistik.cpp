#include "Statistik.h"
#include <iostream>

void Statistik::aktualisiereStatistik(const Charakter& charakter, bool gewonnen)
{
    if (gewonnen)
    {
        gewonneneKaempfe[charakter.name]++;
    }
    else
    {
        verloreneKaempfe[charakter.name]++;
    }
}

void Statistik::zeigeStatistik() const
{
    std::cout << "Gewonnene Kaempfe pro Charakter:\n";
    for (const auto& entry : gewonneneKaempfe)
    {
        std::cout << entry.first << ": " << entry.second << "\n";
    }

    std::cout << "Verlorene Kaempfe pro Charakter:\n";
    for (const auto& entry : verloreneKaempfe)
    {
        std::cout << entry.first << ": " << entry.second << "\n";
    }
}
