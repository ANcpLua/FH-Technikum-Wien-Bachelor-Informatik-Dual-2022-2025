#ifndef ZERSTOERER_H
#define ZERSTOERER_H

#include "Schiff.h"

class Zerstoerer : public Schiff {
public:
    Zerstoerer();
    bool spezialAngriff(Schiff &other) override;
};

#endif //ZERSTOERER_H
