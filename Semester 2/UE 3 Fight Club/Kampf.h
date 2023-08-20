#ifndef KAMPF_H
#define KAMPF_H

#include "Team.h"
#include "Statistik.h"

class Kampf
{
public:
    static void durchfuehrenKampf(Team& team1, Team& team2, Statistik& statistik);
};

#endif // KAMPF_H
