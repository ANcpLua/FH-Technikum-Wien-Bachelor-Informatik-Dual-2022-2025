#include "Tower.h"

Tower::Tower(int numOfSlices)
{
    std::cout << "[*] Tower Constructor" << std::endl;

    this->maxHeight = numOfSlices;
}

Tower::~Tower()
{
    std::cout << "[*] Tower Destructor" << std::endl;

    while(!slices.empty())
    {
        Slice* slice = slices.back();
        slices.pop_back();
        delete slice;
    }
}

bool Tower::AddSlice(Slice* slice)
{
    slices.push_back(slice);

    // NOTE:
    // Gueltigkeit des Zuges wird bereits VOR Funktionsaufruf geprueft
    // Aus Zeitgruenden nicht mehr geaendert - gibt daher immer true zurueck
    // Siehe Hanoi::MoveSlice()

    return true;
}

Slice* Tower::RemoveSlice()
{
    if(slices.size() == 0)
    {
        return nullptr;
    }

    Slice* topSlice = slices.back();
    slices.pop_back();
    return topSlice;
}

int Tower::GetHeight()
{
    return (int)slices.size();
}

std::vector<Slice*> Tower::GetSlices()
{
    return slices;
}
