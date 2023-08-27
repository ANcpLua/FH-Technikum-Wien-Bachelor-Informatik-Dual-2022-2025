#include "RecurringField.h"

RecurringField::RecurringField()
{
    std::cout << "[*] RecurringField Constructor" << std::endl;

    this->points = rand() % 3 + 1;
    this->display = 'O';
}

RecurringField::~RecurringField()
{
    std::cout << "[*] RecurringField Destructor" << std::endl;
}

bool RecurringField::execute()
{
    this->points -= 1;
    return true;
}
