#include "Field.h"

Field::Field(){}

Field::~Field(){}

void Field::displayField()
{
    std::cout << display << " ";
}

int Field::getPoints()
{
    return points;
}
