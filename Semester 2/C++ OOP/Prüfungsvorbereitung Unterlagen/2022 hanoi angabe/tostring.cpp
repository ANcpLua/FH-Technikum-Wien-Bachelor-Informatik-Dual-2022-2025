#include <sstream> // <-- INCLUDE IN THE BEGINNING OF THE FILE!

std::string Hanoi::ToString() const
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
}