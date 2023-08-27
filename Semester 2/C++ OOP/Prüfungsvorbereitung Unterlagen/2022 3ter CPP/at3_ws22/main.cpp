#include <iostream>

#include "Exam.h"
#include "SingleChoiceQuestion.h"
#include "MultipleChoiceQuestion.h"

int main()
{
    try
    {
        int input = 0;

        Exam finalExam;

        //Uncomment this to generate an exam
        Question* temp = nullptr;
        temp = new SingleChoiceQuestion("Single1", 1);
        temp->addAnswer("Antwort 1 (richtig)", true);
        temp->addAnswer("Antwort 2", false);
        finalExam.addQuestion(temp);

        temp = new MultipleChoiceQuestion("Multi1", 1);
        temp->addAnswer("Antwort 1(richtig)", true);
        temp->addAnswer("Antwort 2", false);
        finalExam.addQuestion(temp);

        temp = new SingleChoiceQuestion("Single2", 1);
        temp->addAnswer("Antwort 1(richtig)", true);
        temp->addAnswer("Antwort 2", false);
        temp->addAnswer("Antwort 3", false);
        finalExam.addQuestion(temp);

        temp = new MultipleChoiceQuestion("Multi2", 1);
        temp->addAnswer("Antwort 1(richtig)", true);
        temp->addAnswer("Antwort 2", false);
        temp->addAnswer("Antwort 3(richtig)", true);
        temp->addAnswer("Antwort 4", false);
        finalExam.addQuestion(temp);

        // Test Start
        std::cout << "Willkommen zum Abschlusstest!" << std::endl;

        //Uncomment this to test your code
        while(finalExam.getCurrentQuestion() != nullptr)
        {
            Question* currentQuestion = finalExam.getCurrentQuestion();

            currentQuestion->print();

            do
            {
                std::cout << "Antwort auswaehlen: ";
                std::cin >> input;

                if(input != 0)
                {
                    currentQuestion->markAnswer(input);
                }
            }
            while(input != 0);

            finalExam.nextQuestion();
        }

        std::cout << "Sie haben " << finalExam.grade() << " Punkte erreicht" << std::endl;
    }

    catch(std::invalid_argument& invalidArgument)
    {
        std::cout << invalidArgument.what() << std::endl;
    }

    catch(std::logic_error& logicError)
    {
        std::cout << logicError.what() << std::endl;
    }

    return 0;
}
