// exceptions.cpp
// Author: David Napravnik

#include "ownContainer.h"
#include <iostream>
#include <string>

int main() {
	ownContainer oc(100);
	for (int i = 0; i < 90; i++)
		oc.add(i);

	try {
		std::cout << oc[42] << std::endl;
		std::cout << oc[142] << std::endl;
	}
	catch (...) {
		std::cout << "some error" << std::endl;
	}

	try {
		for (int i = 0; i < 20; i++)	
			oc.add(i);
	}
	catch (...) {
		std::cout << "some error" << std::endl;
	}
}