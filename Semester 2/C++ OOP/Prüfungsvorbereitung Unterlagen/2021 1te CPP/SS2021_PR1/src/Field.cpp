#include "Field.h"

Field::Field()
{
    std::cout << "[*] Field Constructor" << std::endl;
}

Field::~Field()
{
    std::cout << "[*] Field Destructor" << std::endl;
}

void Field::displayField()
{
    std::cout << display << " ";
}

int Field::getPoints()
{
    return points;
}

char Field::getDisplay()
{
    return this->display;
}
