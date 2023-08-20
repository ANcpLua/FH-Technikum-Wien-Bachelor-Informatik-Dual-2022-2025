#include <iostream>
#include "world.h"
#include "Bot.h"
#include <vector>
#include <chrono>
#include <cstdlib>


// World constructor
World::World() : sizeX(5), sizeY(5), sizeZ(10)
{
    for (int x = 0; x < sizeX; x++)
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                worldArray[x][y][z] = rand() % 9 + 1;
            }
        }
    }
}

World::~World() {}

// Marks the mined location with -10 in the worldArray
void World::markMinedLocation(int x, int y, int z)
{
    worldArray[x][y][z] = -10;
}

// Displays the start menu
void World::startMenu()
{
    std::cout << "Let the Game begin!" << std::endl;
    std::cout << "___________________" << std::endl << std::endl;
    showWorld();
}

// Returns a vector with depth values at given x, y, and z
std::vector<int> World::getDepthVector(int x, int y, int z)
{
    std::vector<int> depth;
    for (int i = z; i < sizeZ; i++)
    {
        if (worldArray[x][y][i] != -10)
        {
            depth.push_back(worldArray[x][y][i]);
        } else {
            break;
        }
    }
    return depth;
}

// Shows an error message
void World::showError()
{
    std::cout << std::endl;
    std::cout << "[Error] That is not possible!" << std::endl;
}

// Displays the world
void World::showWorld() {
    for (int z = 0; z < sizeZ; ++z)
    {
        std::cout << "\nLayer " << z << ":" << std::endl;
        for (int y = 0; y < sizeY; ++y)
        {
            for (int x = 0; x < sizeX; ++x)
            {
                std::cout << (worldArray[x][y][z] == -10 ? "M " : std::to_string(worldArray[x][y][z]) + " ");
            }
            std::cout << std::endl;
        }
    }
}

// Implementation of the game loop
void World::play(Bot *player1, Bot *robot)
{
    while (true) {
        playerMove(player1);
        robotMove(robot);

        showWorld();
        displayScores(player1, 1);
        displayScores(robot, 2);

        char exitOption;
        std::cout << "Do you want to exit the game? (y/n): ";
        std::cin >> exitOption;
        if (exitOption == 'y' || exitOption == 'Y') {
            delete player1;
            delete robot;
            break;
        }
    }
}
// Handles player move
void World::playerMove(Bot* playerBot)
{
    int x, y,z;

    while (true)
    {
        std::cout << "Enter X Y Z to dig: ";
        std::cin >> x >> y >> z;
        std::cout << std::endl;

        if (selectAndDig(x, y, z, *playerBot))
        {
            break;
        }
    }
}

// Handles robot move
void World::robotMove(Bot* robotBot)
{
    int x, y, z;
    while (true)
    {
        x = rand() % sizeX;
        y = rand() % sizeY;
        z = rand() % sizeZ;

        if (selectAndDig(x, y, z, *robotBot))
        {
            std::cout << "Robot dug at coordinates (" << x << ", " << y << ", " << z << ")." << std::endl;
            break;
        }
    }
}

// Displays scores for the given player
void World::displayScores(Bot* bot, int player)
{
    std::cout << "Player " << player << " score: " << bot->getScore() << std::endl;
}

// Allows the user to select a bot
Bot* World::selectBot()
{
    int choice;

    while (true)
    {
        std::cout << "Select your Bot:\n";
        std::cout << "1. BotOne\n";
        std::cout << "2. BotTwo\n";
        std::cout << "3. BotThree\n";
        std::cout << "Enter your choice: ";

        std::cin >> choice;
        std::cout << std::endl;

        if (std::cin.fail())
        {
            std::cin.clear();
            std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
        }

        switch (choice) {
            case 1:
                return new BotOne();
            case 2:
                return new BotTwo();
            case 3:
                return new BotThree();
            default:
                std::cout << "Invalid choice. Try again.\n";
        }
    }
}
// Tries to dig at the given x, y, z and returns true if successful
bool World::selectAndDig(int x, int y, int z, Bot &bot)
{

    if (x < 0 || x >= sizeX || y < 0 || y >= sizeY || z < 0 || z >= sizeZ) {
        showError();
        return false;
    }

    std::vector<int> depth = getDepthVector(x, y, z);

    if (depth.empty()) {
        std::cout << "Nothing to dig here!" << std::endl;
        return false;
    }

    int mined_depth = bot.mine(depth, x, y, z, *this); // Pass World object

    for (int i = 0; i < mined_depth; i++)  // value returned from mine
    {
        markMinedLocation(x, y, z);
    }

    return true;
}




Bot::~Bot() {}
BotOne::BotOne() : Bot() {}

int BotOne::mine(std::vector<int> depth, int x, int y, int z, World &world)
{
    int points = depth.front();
    addScore(points);
    std::cout << "BotOne dug layer " << z << " with value " << points << std::endl;
    return 1;
}
BotOne::~BotOne() {}

BotTwo::BotTwo() : Bot() {}
int BotTwo::mine(std::vector<int> depth, int x, int y, int z, World &world)
{
    int points = depth.front();
    addScore(points);
    std::cout << "BotTwo dug layer " << z << " with value " << points << std::endl;
    return 1;
}
BotTwo::~BotTwo() {}

BotThree::BotThree() : Bot() {}
int BotThree::mine(std::vector<int> depth, int x, int y, int z, World &world)
{
    int points = depth.front();
    addScore(points);
    std::cout << "BotThree dug layer " << z << " with value " << points << std::endl;
    return 1;
}
BotThree::~BotThree() {}

