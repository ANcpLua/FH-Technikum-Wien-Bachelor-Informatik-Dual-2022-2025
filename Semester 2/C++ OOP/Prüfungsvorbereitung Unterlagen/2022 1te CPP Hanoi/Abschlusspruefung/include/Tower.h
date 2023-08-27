#ifndef TOWER_H
#define TOWER_H

#include <iostream>
#include <vector>

#include "Slice.h"

class Tower
{
    public:
        Tower(int numOfSlices);
        virtual ~Tower();
        bool AddSlice(Slice* slice);
        Slice* RemoveSlice();
        int GetHeight();
        std::vector<Slice*> GetSlices();

    private:
        std::vector<Slice*> slices;
        int maxHeight;
};

#endif // TOWER_H
