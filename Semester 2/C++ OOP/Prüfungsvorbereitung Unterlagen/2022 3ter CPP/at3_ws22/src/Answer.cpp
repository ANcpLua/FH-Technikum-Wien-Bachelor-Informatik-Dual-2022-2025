#include "Answer.h"

Answer::Answer(std::string name, bool isCorrect)
{
    std::cout << "[*] Answer Constructor" << std::endl;
    this->name = name;
    this->isCorrect = isCorrect;
}

Answer::~Answer()
{
    std::cout << "[*] Answer Destructor" << std::endl;
}

std::string Answer::getName()
{
    return this->name;
}

bool Answer::getIsCorrect()
{
    return this->isCorrect;
}
