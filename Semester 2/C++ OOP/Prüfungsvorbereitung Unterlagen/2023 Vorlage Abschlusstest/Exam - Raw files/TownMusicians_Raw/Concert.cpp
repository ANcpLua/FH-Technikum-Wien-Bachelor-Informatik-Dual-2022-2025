#include <iostream>
#include "Concert.h"
#include "DogSong.h"
#include "CatSong.h"
#include "RoosterSong.h"

Concert::Concert(std::string songString)
{
    for(char s : songString)
    {
        switch(s)
        {
            case 'C': songs.push_back(new CatSong());
            break;
            case 'D': songs.push_back(new DogSong());
            break;
            case 'R': songs.push_back(new RoosterSong());
        }
    }
}