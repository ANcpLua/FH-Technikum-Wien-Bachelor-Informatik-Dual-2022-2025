#include "Slice.h"

Slice::Slice(int size)
{
    std::cout << "[*] Slice Constructor" << std::endl;

    this->size = size;
}

Slice::~Slice()
{
    std::cout << "[*] Slice Destructor" << std::endl;
}

int Slice::GetSize()
{
    return this->size;
}
