#include "MultipleChoiceQuestion.h"

MultipleChoiceQuestion::MultipleChoiceQuestion(std::string name, int pointValue) : Question(name, pointValue)
{
    std::cout << "[*] MultipleChoiceQuestion Constructor" << std::endl;
}

MultipleChoiceQuestion::~MultipleChoiceQuestion()
{
    std::cout << "[*] MultipleChoiceQuestion Destructor" << std::endl;
}

void MultipleChoiceQuestion::markAnswer(int answerNumber)
{
    if(answerNumber < 1 || answerNumber > (int)answers.size())
    {
        throw std::invalid_argument("Invalid Answer selected.");
    }

    for(auto& markedAnswerPositionInVector : this->markedAnswerPositionsInVector)
    {
        if(answerNumber == markedAnswerPositionInVector)
        {
            throw std::logic_error("This answer for question was already provided.");
        }
    }

    this->markedAnswerPositionsInVector.push_back(answerNumber - 1);
}

int MultipleChoiceQuestion::getPoints()
{
    if(this->markedAnswerPositionsInVector.empty())
    {
        return 0;
    }

    int points = 0;

    for(auto& answer : this->answers)
    {
        bool pointsGiven = false;

        if(answer->getIsCorrect())
        {
            for(auto& markedAnswerPositionInVector : this->markedAnswerPositionsInVector)
            {
                if(markedAnswerPositionInVector < 0 || markedAnswerPositionInVector > (int)this->answers.size() - 1)
                {
                    throw std::invalid_argument("Invalid Answer selected.");
                }

                if(!pointsGiven && this->answers.at(markedAnswerPositionInVector) == answer)
                {
                    points += pointValue;
                    pointsGiven = true;
                }
            }

            if(!pointsGiven)
            {
                points -= pointValue;
            }
        }

        if(!answer->getIsCorrect())
        {
            for(auto& markedAnswerPositionInVector : this->markedAnswerPositionsInVector)
            {
                if(markedAnswerPositionInVector < 0 || markedAnswerPositionInVector > (int)this->answers.size() - 1)
                {
                    throw std::invalid_argument("Invalid Answer selected.");
                }

                if(!pointsGiven && this->answers.at(markedAnswerPositionInVector) == answer)
                {
                    points -= pointValue;
                    pointsGiven = true;
                }
            }
        }
    }

    if(points > 0)
    {
        return points;
    }

    return 0;
}
