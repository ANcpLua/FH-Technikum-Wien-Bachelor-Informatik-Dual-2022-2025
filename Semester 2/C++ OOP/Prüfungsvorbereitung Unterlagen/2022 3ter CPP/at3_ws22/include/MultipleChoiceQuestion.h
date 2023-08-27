#ifndef MULTIPLECHOICEQUESTION_H
#define MULTIPLECHOICEQUESTION_H

#include "Question.h"

class MultipleChoiceQuestion : public Question
{
    public:
        MultipleChoiceQuestion(std::string name, int pointValue);
        virtual ~MultipleChoiceQuestion();
        void markAnswer(int answerNumber);
        int getPoints();

    protected:

    private:
        std::vector<int> markedAnswerPositionsInVector;
};

#endif // MULTIPLECHOICEQUESTION_H
