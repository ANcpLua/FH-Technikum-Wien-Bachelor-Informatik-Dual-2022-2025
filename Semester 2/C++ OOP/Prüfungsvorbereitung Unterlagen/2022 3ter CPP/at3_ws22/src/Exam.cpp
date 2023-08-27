#include "Exam.h"

Exam::Exam()
{
    std::cout << "[*] Exam Constructor" << std::endl;
}

Exam::~Exam()
{
    std::cout << "[*] Exam Destructor" << std::endl;

    while (!questions.empty())
    {
        Question* tmp = questions.back();
        questions.pop_back();
        delete tmp;
    }
}

Question* Exam::getCurrentQuestion()
{
    if(questions.empty())
    {
        return nullptr;
    }

    else if(this->currentQuestionPositionInVector < (int)questions.size())
    {
        return questions.at(this->currentQuestionPositionInVector);
    }

    return nullptr;
}

void Exam::addQuestion(Question* newQuestion)
{
    questions.push_back(newQuestion);
}

void Exam::nextQuestion()
{
    this->currentQuestionPositionInVector += 1;
}

int Exam::grade()
{
    int grade = 0;

    for(auto& question : questions)
    {
        grade += question->getPoints();
    }

    return grade;
}
