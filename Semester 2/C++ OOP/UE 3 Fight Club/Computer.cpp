#include "Computer.h"
#include <cstdlib> // for rand() and srand()
#include <ctime> // for time()

Charakter Computer::erstelleZufaelligenCharakter(const std::vector<Faehigkeit>& verfuegbareFaehigkeiten, const std::vector<std::string>& verfuegbareElemente)
{
    std::string name = "Gegner";
    int lebenspunkte = 1000;
    const std::string& element = verfuegbareElemente[std::rand() % verfuegbareElemente.size()];
    std::vector<Faehigkeit> faehigkeiten = {
            verfuegbareFaehigkeiten[std::rand() % verfuegbareFaehigkeiten.size()],
            verfuegbareFaehigkeiten[std::rand() % verfuegbareFaehigkeiten.size()]
    };

    return Charakter(name, lebenspunkte, element, faehigkeiten);
}
