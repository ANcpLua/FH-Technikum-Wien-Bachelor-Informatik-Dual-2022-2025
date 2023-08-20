#include <iostream>
#include <string>
#include <vector>
#include <ctime>
#include <algorithm>
#include <limits>
#include <cstdlib>


#include "Charakter.h"
#include "Faehigkeit.h"
#include "Team.h"
#include "Statistik.h"
#include "Computer.h"
#include "Kampf.h"
#include "Character_builder.h"


void waehleCharakter(const std::string &auswahl, Team &team, const Charakter &kaido, const Charakter &ruffy)
{
    if (auswahl == "Kaido")
    {
        team.hinzufuegenCharakter(kaido);
        std::cout << "Kaido zum Team hinzugefuegt.\n";
    }
    else if (auswahl == "Ruffy")
    {
        team.hinzufuegenCharakter(ruffy);
        std::cout << "Ruffy zum Team hinzugefuegt.\n";
    }
    else
    {
        std::cout << "Ungueltige Auswahl. Bitte versuchen Sie es erneut.\n";
    }
}


int main()
{
    srand(time(nullptr));

    std::vector<Faehigkeit> verfuegbareFaehigkeiten = {Faehigkeit("Schlag", 10),Faehigkeit("Kick", 20),Faehigkeit("Hieb", 30),Faehigkeit("Punch", 40),Faehigkeit("Hit", 50),Faehigkeit("Drag", 60)};

    std::vector<std::string> verfuegbareElemente = {"Wasser", "Feuer", "Blatt", "Eis"};

    Charakter kaido("Kaido", 100, "Feuer", {verfuegbareFaehigkeiten[0], verfuegbareFaehigkeiten[1]});
    Charakter ruffy("Ruffy", 100, "Wasser", {verfuegbareFaehigkeiten[2], verfuegbareFaehigkeiten[3]});

    Team team1, team2;
    Statistik statistik;

    bool weitermachen = true;
    while (weitermachen)
    {
        std::cout << "Bitte waehlen Sie aus zwischen Erstellen, Vorgefertigter Character Kaido oder Ruffy.\nSobald sie einen oder mehrere Character erstellt oder ausgewaehlt haben waehlen sie zwischen 1vs1 oder 2vs2 oder schreiben sie Beenden:\n";
        std::string auswahl;
        std::cin >> auswahl;
        if (auswahl == "Erstellen")
        {
            std::string name, element;
            std::cout << "Bitte geben Sie irgendeinen Namen und element(Wasser,Feuer,Eis,Blatt) ein fuer ihren Fighter:\n";

            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
            std::getline(std::cin, name);

            do {
                std::cout << "Bitte geben Sie das Element ein (Wasser, Feuer, Eis, Blatt): ";
                std::cin >> element;
                std::transform(element.begin(), element.end(), element.begin(), ::tolower);
            } while (element != "wasser" && element != "feuer" && element != "eis" && element != "blatt");

            std::vector<Faehigkeit> charakterFaehigkeiten;
            while (charakterFaehigkeiten.size() < 2)
            {
                for (const auto &faehigkeit : verfuegbareFaehigkeiten)
                {
                    std::cout << "Moechten Sie die Faehigkeit \"" << faehigkeit.name << "\" hinzufuegen? (j/n)\n";
                    char auswahlFaehigkeit;
                    std::cin >> auswahlFaehigkeit;
                    if (auswahlFaehigkeit == 'j' || auswahlFaehigkeit == 'J')
                    {
                        charakterFaehigkeiten.push_back(faehigkeit);
                    }
                }

                if (charakterFaehigkeiten.size() < 2)
                {
                    std::cout << "Bitte waehlen Sie mindestens 2 Faehigkeiten aus.\n";
                }
            }

            Charakter erstellterCharakter = CharacterBuilder::erstelleCharakter(name, element, charakterFaehigkeiten);
            team1.hinzufuegenCharakter(erstellterCharakter);
            std::cout << "Charakter erstellt und zum Team hinzugefuegt.\n";
        }
        else if (auswahl == "Kaido" || auswahl == "Ruffy")
        {
            waehleCharakter(auswahl, team1, kaido, ruffy);
        }
        else if (auswahl == "1vs1" || auswahl == "2vs2")
        {
            size_t anzahlCharaktere = auswahl == "1vs1" ? 1 : 2;
            if (team1.charaktere.size() != anzahlCharaktere)
            {
                std::cout << "Bitte stellen Sie sicher, dass Ihr Team " << anzahlCharaktere << " Charaktere hat, bevor Sie fortfahren.\n";
                continue;
            }


            Team gegnerTeam;
            for (size_t i = 0; i < anzahlCharaktere; ++i)
            {
                Charakter zufaelligerCharakter = Computer::erstelleZufaelligenCharakter(verfuegbareFaehigkeiten, verfuegbareElemente);
                gegnerTeam.hinzufuegenCharakter(zufaelligerCharakter);
            }

            Kampf::durchfuehrenKampf(team1, gegnerTeam, statistik);
            statistik.zeigeStatistik();

            team1.leeren();
        }
        else if (auswahl == "Beenden")
        {
            weitermachen = false;
        }
        else
        {
            std::cout << "Ungueltige Auswahl. Bitte versuchen Sie es erneut.\n";
        }
    }

    return 0;
}
