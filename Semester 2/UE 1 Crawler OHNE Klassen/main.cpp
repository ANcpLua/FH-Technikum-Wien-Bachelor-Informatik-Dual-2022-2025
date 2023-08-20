#include <iostream>
#include <cstdlib>      //rand() srand
#include <ctime>        // zeit f�rs seed

const int SIZE = 5;

struct Player
{
    int x, y;
    int life, relics;
};

char randomlyGenerateField()            //0-9 rand       0-3 space, 4 R, 5 F und 6-9 X     ternary operator vergleicht wie if else
{
    int random = rand() % 10;
    return (random < 4) ? ' ' : (random == 4) ? 'R' : (random == 5) ? 'F' : 'X';        // (0,9)^25 f�r kein relic aber keine zeit mehr gehabt ums zu fixen
}

void initializeWorld(char world[SIZE][SIZE])
{
    for (int i = 0; i < SIZE; i++)
    {
        for (int j = 0; j < SIZE; j++)
        {
            world[i][j] = randomlyGenerateField();
        }
    }
    world[0][0] = 'P';      // starting point links oben
}

void displayGame(char world[SIZE][SIZE], Player p)      // davinci ich wei�
{
    for (int i = 0; i < SIZE; i++)
    {
        std::cout << "+---";
    }
    std::cout << "+" << std::endl;

    for (int i = 0; i < SIZE; i++)
    {
        for (int j = 0; j < SIZE; j++)
        {
            std::cout << "| " << world[i][j] << " ";
        }
        std::cout << "|" << std::endl;

        for (int j = 0; j < SIZE; j++)
        {
            std::cout << "+---";
        }
        std::cout << "+" << std::endl;
    }
    std::cout << "Player lives left: " << p.life << std::endl;
    std::cout << "Relics collected: " << p.relics << std::endl;
    std::cout << std::endl;
}

void movePlayerInWorld(Player& p, char world[SIZE][SIZE], int x, int y, std::string events[], int& numEvents)
{
    world[p.x][p.y] = ' ';
    if (world[x][y] == 'R')
    {
        events[numEvents++] = "Found a relic.";
        p.relics++;
    }
    else if (world[x][y] == 'F')
    {
        events[numEvents++] = "Gained a life.";
        p.life++;

    }
    else if (world[x][y] == 'X')
    {
        events[numEvents++] = "Stepped into danger.";
        if (rand() % 6 + 1 == 6)            // +1 damit == nicht immer triggert    6+1 = 1-6 also 1/6 chance
        {
            events[numEvents++] = "Stepped into danger and lost a life.";
            p.life--;
        }
    }
    world[x][y] = 'P';      // anzeige vom 'P'
    p.x = x;
    p.y = y;
}

void movePlayer(Player& p, char world[SIZE][SIZE], std::string events[], int& numEvents)
{
    char dir;
    while (true)
    {
        std::cout << "Enter the direction to move the player (U,D,L,R): " << std::endl;
        std::cin >> dir;

        int x = p.x;
        int y = p.y;

        switch (dir)                // error handlers, boring
        {
        case 'U':
        case 'u':
            if (x == 0)         //wenn [0] [0] dann ist man ganz oben und also U dr�cken wenn man ganz oben ist= falsch
            {
                std::cout << "Invalid move" << std::endl;
                continue;
            }
            movePlayerInWorld(p, world, x - 1, y, events, numEvents);       // wenn x nicht 0 oder negativ ist kommmt mann in diese zeile und geht rauf zb [2] [0] -> [1] [0] = 1 rauf
            return;
        case 'D':
        case 'd':
            if (x == 4)         // ganz unten angekommen ist [4] [0}  das unterste also jetzt D dr�cken= falsch
            {
                std::cout << "Invalid move" << std::endl;
                continue;
            }
            movePlayerInWorld(p, world, x + 1, y, events, numEvents);        // wenn x nicht 4 oder mehr ist kommmt mann in diese zeile und geht runter zb [2] [0] -> [3] [0] = 1 runter
            return;
        case 'L':
        case 'l':
            if (y == 0)             // wenn y 0 sind wir ganz links angekommen
            {
                std::cout << "Invalid move" << std::endl;
                continue;
            }
            movePlayerInWorld(p, world, x, y - 1, events, numEvents);       // wenn nicht 0 oder negativ dann sind wir nicht ganz links und gehen [2] [2] -> [2] [1} nach links
            return;
        case 'R':
        case 'r':
            if (y == 4)     // wenn y 4 ist sind wir ganz rechts angekommen
            {
                std::cout << "Invalid move" << std::endl;
                continue;
            }
            movePlayerInWorld(p, world, x, y + 1, events, numEvents);       // wenn nicht 4 oder mehr dann sind wir nicht ganz rechts und gehen [2] [2] -> [2] [3} nach rechts
            return;
        default:            // wenn nicht U u D d L l oder R r
            std::cout << "Invalid move" << std::endl;
            continue; // damits weitergeht
        }
    }
}

int gameStatus(Player p, char world[SIZE][SIZE])        // z�hlt relics und leben wenn 0 leben return -1 = game over, wenn return 1 keine relics mehr = win
{
    // und ja ich h�tte bool usen k�nnen
    int remainingRelics = 0;
    for (int i = 0; i < SIZE; i++)
    {
        for (int j = 0; j < SIZE; j++)
        {
            if (world[i][j] == 'R')
            {
                remainingRelics++;
            }
        }
    }

    if (p.life <= 0)
    {
        return -1;
    }
    else if (remainingRelics == 0)
    {
        return 1;
    }
    else
    {
        return 0;
    }
}

int main()
{
    std::cout << "P(Spieler)  You die with 0 life" << std::endl;
    std::cout << "X(Gefahren) If you step into X you have a 1/6 chance to lose 1 life" << std::endl;
    std::cout << "F(Brunnen)  Heals you for 1 life" << std::endl;
    std::cout << "R(Relikte)  Collect all Relics to win the game!" << std::endl;
    std::cout << std::endl;
    srand((int)time(NULL)); // zeit 0 deshalb immer anders
    char world[SIZE][SIZE] = {0};
    initializeWorld(world);
    Player player = {0, 0, 5, 0};       // x 0 y 0 5 leben 0 relics
    std::string events[100];         // speichert die events
    int numEvents = 0;
    int status = gameStatus(player, world);
    while (status == 0)     // solang return 0 passt alles
    {
        displayGame(world, player);
        movePlayer(player, world, events, numEvents);
        status = gameStatus(player, world);
    }
    displayGame(world, player);
    if (status == 1)
        std::cout << "Congratulations! You won the game!" << std::endl;
    else
        std::cout << "You lost the game!" << std::endl;
    std::cout << std::endl << "Game events:" << std::endl;
    for (int i = 0; i < numEvents; i++)
        std::cout << i + 1 << ": " << events[i] << std::endl;       // ja h�tte auch nur i sein k�nnen aber i+1 wei� ich erstes event weil 0 klingt bl�d
    return 0;
}
