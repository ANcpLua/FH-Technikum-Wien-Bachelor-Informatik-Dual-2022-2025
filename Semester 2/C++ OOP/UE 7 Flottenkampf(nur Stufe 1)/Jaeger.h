#ifndef JAEGER_H
#define JAEGER_H

#include "Schiff.h"

class Jaeger : public Schiff {
public:
    Jaeger();
    bool spezialAngriff(Schiff& other) override;
};

#endif //JAEGER_H
