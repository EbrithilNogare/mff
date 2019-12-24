// PolyContainer.cpp
// Author: David Napravnik

#include "PolyContainer.hpp"

using namespace std;

void PolyContainer::print(std::ostream& s, char separator) {
	for (auto && it = container_.begin(); it != container_.end(); it++) {
		it[0]->print(s);
		if (next(it) != container_.end())
			s << separator;
	}
}
