#ifndef SINGLECHOICEQUESTION_H
#define SINGLECHOICEQUESTION_H

#include "Question.h"

class SingleChoiceQuestion : public Question
{
    public:
        SingleChoiceQuestion(std::string name, int pointValue);
        virtual ~SingleChoiceQuestion();
        void markAnswer(int answerNumber);
        int getPoints();

    protected:

    private:
        int markedAnswerPositionInVector = -1;
};

#endif // SINGLECHOICEQUESTION_H
