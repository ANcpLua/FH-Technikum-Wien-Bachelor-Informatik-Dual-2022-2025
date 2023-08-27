#ifndef CONCERT_H
#define CONCERT_H

#include <vector>
#include "IAnimalSong.h"

class Concert
{
    public:
        Concert(std::string);
        ~Concert();
        int play();

    protected:

    private:
        std::vector<IAnimalSong*> songs;
};

#endif // CONCERT_H
