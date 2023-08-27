#include "SingleChoiceQuestion.h"

SingleChoiceQuestion::SingleChoiceQuestion(std::string name, int pointValue) : Question(name, pointValue)
{
    std::cout << "[*] SingleChoiceQuestion Constructor" << std::endl;
}

SingleChoiceQuestion::~SingleChoiceQuestion()
{
    std::cout << "[*] SingleChoiceQuestion Destructor" << std::endl;
}

void SingleChoiceQuestion::markAnswer(int answerNumber)
{
    if(answerNumber < 1 || answerNumber > (int)answers.size())
    {
        throw std::invalid_argument("Invalid Answer selected.");
    }

    if(this->markedAnswerPositionInVector != -1)
    {
        throw std::logic_error("An answer for this question was already provided.");
    }

    this->markedAnswerPositionInVector = answerNumber - 1;
}

int SingleChoiceQuestion::getPoints()
{
    if(this->markedAnswerPositionInVector == -1)
    {
        return 0;
    }

    if(this->markedAnswerPositionInVector < 0 || this->markedAnswerPositionInVector > (int)answers.size() - 1)
    {
        throw std::invalid_argument("Invalid Answer selected.");
    }

    if(answers.at(this->markedAnswerPositionInVector)->getIsCorrect())
    {
        return this->pointValue;
    }

    return 0;
}
