#ifndef IHANOI_H
#define IHANOI_H

#include <iostream>

class IHanoi
{
    public:
        virtual void MoveSlice(int from, int to) = 0;
        virtual bool IsComplete() = 0;
        virtual std::string ToString() = 0;
};

#endif // IHANOI_H
