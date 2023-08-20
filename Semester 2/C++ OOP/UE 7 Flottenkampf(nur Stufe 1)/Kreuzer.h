#ifndef KREUZER_H
#define KREUZER_H

#include "Schiff.h"

class Kreuzer : public Schiff {
public:
    Kreuzer();
    bool spezialAngriff(Schiff& other) override;
};

#endif //KREUZER_H
