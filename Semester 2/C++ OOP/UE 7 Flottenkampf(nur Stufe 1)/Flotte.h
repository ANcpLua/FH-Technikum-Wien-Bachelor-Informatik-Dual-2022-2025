#ifndef FLOTTE_H
#define FLOTTE_H

#include <string>
#include <vector>
#include "Schiff.h"

class Flotte {
public:
    explicit Flotte(std::string name);

    void hinzufuegenSchiff(Schiff* schiff);
    void entfernenSchiff(Schiff* schiff);
    bool istZerstoert() const;
    Schiff* getSchiff(int index);
    ~Flotte();

    std::string name;
    std::vector<Schiff*> schiffe;
};

#endif //FLOTTE_H
