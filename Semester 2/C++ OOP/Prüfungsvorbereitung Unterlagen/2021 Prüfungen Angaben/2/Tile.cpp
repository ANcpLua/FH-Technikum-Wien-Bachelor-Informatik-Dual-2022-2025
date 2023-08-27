#include "Tile.h"

Tile::Tile(){}

Tile::~Tile(){}

bool Tile::isMarked()
{
    return marked;
}

void Tile::display()
{
    std::cout << 'X' << " ";
}
