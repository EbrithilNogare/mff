// main.cpp
// Author: David Napravnik
#include <string>
#include <iostream>
#include "main.h"


int main() {

	WordFrequention(std::cin);

	return 0;
}

void WordFrequention(std::istream& input) {
	// declare variables
	std::pair<std::string, int> dictionary;
	char c;
	std::string word;


	// read input
	while(true){
		c = input.get();
		if (input.fail())
			return;

		if (isalpha(c))
			word += c;
		else if (word.length != 0) {
			dictionary[word]++;
			word = "";
			}

	}

}