#pragma once

#include <vector>
#include "world.h"

class World;

class Bot{
public:
    Bot() : score(0) {}
    virtual ~Bot() = 0;

    int getScore() const { return score; }
    void addScore(int points) { score += points; }
    virtual int mine(std::vector<int> depth, int x, int y, int z, World &world) = 0;

private:
    int score;
};

class BotOne : public Bot {
public:
    BotOne();
    ~BotOne() override;

    int mine(std::vector<int> depth, int x, int y, int z, World &world) override;
};

class BotTwo : public Bot {
public:
    BotTwo();
    ~BotTwo() override;

    int mine(std::vector<int> depth, int x, int y, int z, World &world) override;
};

class BotThree : public Bot {
public:
    BotThree();
    ~BotThree() override;

    int mine(std::vector<int> depth, int x, int y, int z, World &world) override;
};
