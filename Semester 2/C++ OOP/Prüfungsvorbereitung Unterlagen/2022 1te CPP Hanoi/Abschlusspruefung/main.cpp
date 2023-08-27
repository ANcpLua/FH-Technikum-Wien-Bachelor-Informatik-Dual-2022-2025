#include <iostream>

#include "Hanoi.h"
#include <iostream>

void solveHanoi();

int main()
{
    try
    {
        // Use this, to check the base logic of the game. " >> Game solved << " means, the logic works!
        solveHanoi();

        Hanoi* game = new Hanoi(3, 3);

        std::cout << game << std::endl;

        while (!game->IsComplete()) {
            int from, to;

            std::cout << "Move from : " << std::endl;
            std::cout << " >> ";
            std::cin >> from;
            std::cout << "Move to : " << std::endl;
            std::cout << " >> ";
            std::cin >> to;

            system("cls");
            game->MoveSlice(from, to);
            std::cout << game << std::endl;
        }

        delete game;
    }

    catch(const std::out_of_range& except)
    {
        std::cout << except.what() << std::endl;
    }

    catch(const std::invalid_argument& except)
    {
        std::cout << except.what() << std::endl;
    }

    catch(...)
    {
        std::cout << "[!] An unknown Error occurred." << std::endl;
    }

	return 0;
}

void solveHanoi() {
	Hanoi* game = new Hanoi(3, 5);

	game->MoveSlice(0, 2);
	game->MoveSlice(0, 1);
	game->MoveSlice(2, 1);
	game->MoveSlice(0, 2);
	game->MoveSlice(1, 0);
	game->MoveSlice(1, 2);
	game->MoveSlice(0, 2);
	game->MoveSlice(0, 1);
	game->MoveSlice(2, 1);
	game->MoveSlice(2, 0);
	game->MoveSlice(1, 0);
	game->MoveSlice(2, 1);
	game->MoveSlice(0, 2);
	game->MoveSlice(0, 1);
	game->MoveSlice(2, 1);
	game->MoveSlice(0, 2);
	game->MoveSlice(1, 2);
	game->MoveSlice(1, 0);
	game->MoveSlice(2, 0);
	game->MoveSlice(1, 2);
	game->MoveSlice(0, 1);
	game->MoveSlice(0, 2);
	game->MoveSlice(1, 2);
	game->MoveSlice(2, 1);
	game->MoveSlice(2, 0);
	game->MoveSlice(1, 0);
	game->MoveSlice(2, 1);
	game->MoveSlice(0, 1);
	game->MoveSlice(0, 2);
	game->MoveSlice(1, 2);
	game->MoveSlice(1, 0);
	game->MoveSlice(2, 1);
	game->MoveSlice(2, 0);
	game->MoveSlice(1, 0);
	game->MoveSlice(1, 2);
	game->MoveSlice(0, 2);
	game->MoveSlice(0, 1);
	game->MoveSlice(2, 1);
	game->MoveSlice(0, 2);
	game->MoveSlice(1, 0);
	game->MoveSlice(1, 2);
	game->MoveSlice(0, 2);

	std::cout << game->ToString() << std::endl;
	std::cout << " >> " << (game->IsComplete() ? "Game solved" : "Game not solved") << " << " << std::endl;

	delete game;

}
