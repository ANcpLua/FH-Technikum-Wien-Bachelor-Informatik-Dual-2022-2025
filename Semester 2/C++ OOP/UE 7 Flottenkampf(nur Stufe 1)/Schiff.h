#ifndef SCHIFF_H
#define SCHIFF_H

#include <string>
#include <windows.h> // for SetConsoleTextAttribute()

class Schiff {
public:
    Schiff(std::string name, int huellenwert, int groesse, int schaden, bool zerstoert = false);

    virtual bool spezialAngriff(Schiff& other);

    bool istZerstoert() const;

    virtual ~Schiff();

    std::string name;
    int huellenwert;
    int groesse;
    int schaden;
    bool zerstoert;
};

#endif //SCHIFF_H
