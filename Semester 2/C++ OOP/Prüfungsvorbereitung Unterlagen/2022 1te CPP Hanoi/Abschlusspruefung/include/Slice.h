#ifndef SLICE_H
#define SLICE_H

#include <iostream>

class Slice
{
    public:
        Slice(int size);
        virtual ~Slice();
        int GetSize();

    private:
        int size;
};

#endif // SLICE_H
