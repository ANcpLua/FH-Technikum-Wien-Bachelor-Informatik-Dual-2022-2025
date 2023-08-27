#include "Question.h"
#include <iostream>

Question::Question(std::string name, int pointValue)
{
    std::cout << "[*] Question Constructor" << std::endl;
    this->name = name;
    this->pointValue = pointValue;
}

Question::~Question()
{
    std::cout << "[*] Question Destructor" << std::endl;

    while (!answers.empty())
    {
        Answer* tmp = answers.back();
        answers.pop_back();
        delete tmp;
    }
}

void Question::print()
{
    std::cout << name << std::endl;

    for(auto& answer : answers)
    {
        std::cout << answer->getName() << std::endl;
    }
}

void Question::addAnswer(std::string name, bool isCorrect)
{
    this->answers.push_back(new Answer(name, isCorrect));
}
