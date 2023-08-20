#ifndef STATISTIK_H
#define STATISTIK_H

#include <map>
#include <string>
#include "Charakter.h"

class Statistik
{
public:
    void aktualisiereStatistik(const Charakter& charakter, bool gewonnen);
    void zeigeStatistik() const;

private:
    std::map<std::string, int> gewonneneKaempfe;
    std::map<std::string, int> verloreneKaempfe;
};

#endif // STATISTIK_H
