#include "Kampf.h"
#include <iostream>
#include <algorithm>
#include <cstdlib> // for rand()

double berechneSchadensmultiplikator(const std::string& angreiferElement, const std::string& verteidigerElement)
{
    if (angreiferElement == "Feuer" && verteidigerElement == "Blatt") return 1.5;
    if (angreiferElement == "Blatt" && verteidigerElement == "Wasser") return 1.5;
    if (angreiferElement == "Eis" && verteidigerElement == "Blatt") return 1.5;
    if (angreiferElement == "Wasser" && verteidigerElement == "Feuer") return 1.5;
    return 1.0;
}

void Kampf::durchfuehrenKampf(Team& team1, Team& team2, Statistik& statistik)
{
    bool team1Turn = true;
    size_t index1 = 0;
    size_t index2 = 0;

    while (!team1.charaktere.empty() && !team2.charaktere.empty())
    {
        Charakter& angreifer = team1Turn ? team1.charaktere[index1] : team2.charaktere[index2];
        Charakter& verteidiger = team1Turn ? team2.charaktere[index2] : team1.charaktere[index1];

        int faehigkeitIndex = std::rand() % angreifer.faehigkeiten.size();
        Faehigkeit& faehigkeit = angreifer.faehigkeiten[faehigkeitIndex];

        double multiplikator = berechneSchadensmultiplikator(angreifer.element, verteidiger.element);
        int schaden = static_cast<int>(faehigkeit.schaden * multiplikator);

        verteidiger.lebenspunkte -= schaden;
        std::cout << angreifer.name << " greift " << verteidiger.name << " mit " << faehigkeit.name << " an und fuegt " << schaden << " Schaden zu.\n";
        if (verteidiger.istTot())
        {
            if (team1Turn)
            {
                std::cout << team2.charaktere[index2].name << " ist besiegt.\n";
                statistik.aktualisiereStatistik(team2.charaktere[index2], false);
                team2.charaktere.erase(team2.charaktere.begin() + index2);
            }
            else
            {
                std::cout << team1.charaktere[index1].name << " ist besiegt.\n";
                statistik.aktualisiereStatistik(team1.charaktere[index1], false);
                team1.charaktere.erase(team1.charaktere.begin() + index1);
            }
        }
        else
        {
            if (team1Turn)
            {
                index2 = (index2 + 1) % team2.charaktere.size();
            }
            else
            {
                index1 = (index1 + 1) % team1.charaktere.size();
            }
        }
        team1Turn = !team1Turn;
    }

    bool team1Gewonnen = !team1.charaktere.empty();
    if (team1Gewonnen)
    {
        std::cout << "Team 1 hat gewonnen!\n";
    }
    else
    {
        std::cout << "Team 2 hat gewonnen!\n";
    }
    statistik.aktualisiereStatistik(team1.charaktere[0], team1Gewonnen);
}

