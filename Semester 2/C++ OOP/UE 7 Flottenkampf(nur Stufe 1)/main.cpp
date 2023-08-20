#include "Spiel.h"
#include <ctime>
#include <cstdlib>

int main() {

    srand(static_cast<unsigned>(time(0)));
    Spiel spiel;

    PlaySound("C:/Users/alex_/Desktop/a.wav", NULL, SND_FILENAME | SND_ASYNC);

    spiel.start();

    return 0;
}
