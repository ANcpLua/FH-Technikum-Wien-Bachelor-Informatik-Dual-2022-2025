#ifndef HANOI_H
#define HANOI_H

#include <iostream>
#include <sstream>
#include <vector>

#include "IHanoi.h"
#include "Tower.h"

class Hanoi : public IHanoi
{
    public:
        Hanoi(int numOfTowers, int numOfSlices);
        virtual ~Hanoi();
        void MoveSlice(int from, int to);
        bool IsComplete();
        std::string ToString();
        void print();

        friend std::ostream& operator << (std::ostream& os, Hanoi* hanoi);

    private:
        std::vector<Tower*> towers;
        int numOfSlices;
        int numOfTowers;
};

#endif // HANOI_H
