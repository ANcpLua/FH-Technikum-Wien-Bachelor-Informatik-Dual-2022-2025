#ifndef FAEHIGKEIT_H
#define FAEHIGKEIT_H

#include <string>

class Faehigkeit
{
public:
    std::string name;
    int schaden;

    Faehigkeit(std::string name, int schaden);
};

#endif // FAEHIGKEIT_H
