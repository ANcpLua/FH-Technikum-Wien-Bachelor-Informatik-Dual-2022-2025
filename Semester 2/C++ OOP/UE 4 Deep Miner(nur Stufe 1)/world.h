#pragma once

#include <iostream>
#include <algorithm>
#include <vector>
#include <chrono>
#include "Bot.h"

class World{
private:
    int worldArray[5][5][10]{};
    int sizeX;
    int sizeY;
    int sizeZ;

public:
    World();
    ~World();

    void startMenu();
    void showError();
    void markMinedLocation(int x, int y, int z);
    void showWorld();
    void play(Bot *player1, Bot *robot);
    static void displayScores(Bot *bot, int player);
    Bot *selectBot();
    void playerMove(Bot *playerBot);
    void robotMove(Bot *robotBot);
    std::vector<int> getDepthVector(int x, int y, int z);
    bool selectAndDig(int x, int y, int z, Bot &bot);
};
