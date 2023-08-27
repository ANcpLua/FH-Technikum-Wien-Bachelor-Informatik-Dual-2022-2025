#include "PlainField.h"

PlainField::PlainField()
{
    std::cout << "[*] PlainField Constructor" << std::endl;

    this->points = 0;
    this->display = '_';
}

PlainField::~PlainField()
{
    std::cout << "[*] PlainField Destructor" << std::endl;
}

bool PlainField::execute()
{
    return false;
}
