#include <iostream>
#include "Concert.h"

int main()
{
    // Create a new concert
    // C = CatSong
    // D = DogSong
    // R = RoosterSong
    Concert concert = Concert("DDDDCR");

    // Let the band play
    int score = concert.play();

    // Print final score, value in parenthesis is the expected score
    cout << "The band reached a score of " << score << " (7) "<< endl;

    concert = Concert("DDD");
    score = concert.play();
    cout << "The band reached a score of " << score << " (3) "<< endl;

    concert = Concert("CCC");
    score = concert.play();
    cout << "The band reached a score of " << score << " (15) "<< endl;

    concert = Concert("R");
    score = concert.play();
    cout << "The band reached a score of " << score << " (3) "<< endl;

    concert = Concert("DRC");
    score = concert.play();
    cout << "The band reached a score of " << score << " (8) "<< endl;

    concert = Concert("RRR");
    score = concert.play();
    cout << "The band reached a score of " << score << " (0) "<< endl;

    concert = Concert("CDC");
    score = concert.play();
    cout << "The band reached a score of " << score << " (8) "<< endl;

    return 0;
}
