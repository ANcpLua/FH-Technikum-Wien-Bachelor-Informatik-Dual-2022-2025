#ifndef QUESTION_H
#define QUESTION_H

#include <iostream>
#include <vector>

#include "Answer.h"

class Question{
    public:
        Question(std::string name, int pointValue);
        virtual ~Question();
        void print();
        void addAnswer(std::string name, bool isCorrect);
        virtual void markAnswer(int answerNumber) = 0;
        virtual int getPoints() = 0;

    protected:
        std::string name;
        int pointValue;
        std::vector<Answer*> answers;
};

#endif // QUESTION_H
