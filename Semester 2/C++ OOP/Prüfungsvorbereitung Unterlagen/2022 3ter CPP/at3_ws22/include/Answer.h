#ifndef ANSWER_H
#define ANSWER_H

#include <iostream>

class Answer
{
    public:
        Answer(std::string name, bool isCorrect);
        virtual ~Answer();
        std::string getName();
        bool getIsCorrect();

    protected:

    private:
        std::string name;
        bool isCorrect;
};

#endif // ANSWER_H
