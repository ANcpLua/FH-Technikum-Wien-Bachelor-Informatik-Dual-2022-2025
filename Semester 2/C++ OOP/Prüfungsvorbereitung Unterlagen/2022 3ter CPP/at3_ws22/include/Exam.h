#ifndef EXAM_H
#define EXAM_H
#include <vector>
#include "Question.h"


class Exam{
    public:
        Exam();
        ~Exam();
        Question* getCurrentQuestion();
        void addQuestion(Question* newQuestion);
        void nextQuestion();
        int grade();

    private:
        std::vector<Question*> questions;
        int currentQuestionPositionInVector = 0;
};

#endif // EXAM_H
