#include "world.h"
#include <chrono>
#include "Bot.h"
#include <cstdlib>


int main() {
    srand(time(0));

    World myWorld;

    myWorld.startMenu();

    Bot* player1 = myWorld.selectBot();

    Bot* robot = myWorld.selectBot();

    myWorld.play(player1, robot);

    return 0;
}
