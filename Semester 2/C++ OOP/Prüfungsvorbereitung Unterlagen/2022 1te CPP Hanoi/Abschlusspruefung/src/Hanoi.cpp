#include "Hanoi.h"

Hanoi::Hanoi(int numOfTowers, int numOfSlices)
{
    std::cout << "[*] Hanoi Constructor" << std::endl;

    for(int i = 0; i < numOfTowers; ++i)
    {
        Tower* tower = new Tower(numOfSlices);

        if(i == 0)
        {
            for(int j = numOfSlices; j > 0; --j)
            {
                Slice* slice = new Slice(j);
                tower->AddSlice(slice);
            }
        }

        this->towers.push_back(tower);
    }

    this->numOfSlices = numOfSlices;
    this->numOfTowers = numOfTowers;
}

Hanoi::~Hanoi()
{
    std::cout << "[*] Hanoi Destructor" << std::endl;

    while(!towers.empty())
    {
        Tower* tower = towers.back();
        towers.pop_back();
        delete tower;
    }
}

void Hanoi::MoveSlice(int from, int to)
{
    if(from == to)
    {
        throw std::invalid_argument("[!] Same Tower chosen twice!");
    }

    if(from < 0 || to < 0 || from > numOfTowers - 1 || to > numOfTowers - 1)
    {
        throw std::out_of_range("[!] One of your parameters was out of bounds!");
    }

    Tower* Tfrom = towers.at(from);
    Tower* Tto = towers.at(to);

    if(Tfrom->GetHeight() <= 0)
    {
        throw std::invalid_argument("[!] The selected tower does not have any slices to move!");
    }

    std::vector<Slice*> Sfrom = Tfrom->GetSlices();
    std::vector<Slice*> Sto = Tto->GetSlices();

    if(Sto.size() == 0 || Sfrom.back()->GetSize() < Sto.back()->GetSize())
    {
        Tto->AddSlice(Tfrom->RemoveSlice());
    }

    else
    {
        throw std::invalid_argument("[!] Invalid Game move: disk is too big.");
    }
}

bool Hanoi::IsComplete()
{
    return towers.at(towers.size() - 1)->GetHeight() == this->numOfSlices;
}

std::string Hanoi::ToString()
{
	std::stringstream ss;

	for (auto tower : this->towers) {
		auto slices = tower->GetSlices();
		for (auto slice = slices.rbegin(); slice != slices.rend(); slice++) {
			ss << (*slice)->GetSize() << std::endl;
		}
		ss << "-" << std::endl << std::endl;
	}

	return ss.str();

	/*
	std::stringstream ss;

	for (int i = 0; i < this->numOfTowers; ++i) {
		std::vector<Slice*> slices = towers.at(i)->GetSlices();
		for (int j = 0; j < this->towers.at(i)->GetHeight(); ++j) {
			ss << this->towers.at(i)->GetSlices().at(j)->GetSize() << std::endl;
		}
		ss << "-" << std::endl << std::endl;
	}

	return ss.str();
	*/
}

std::ostream& operator << (std::ostream& os, Hanoi* hanoi)
{
    std::cout << hanoi->ToString();

    return os;
}
