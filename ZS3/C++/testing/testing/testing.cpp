// File: testing.cpp
// Author: David Napravnik

#include <iostream>
#include <string>
#include <vector>
#include <map>

int main()
{
	// todo make some CUI
}

std::multimap<std::string, std::string> dictionary;

void addTranslate(std::string *word, std::string *translation) {
	dictionary.insert(std::pair<std::string, std::string>(*word, *translation));
}

void removeTranslate(std::string *word, int occurence = 1) {
	std::multimap<std::string, std::string>::iterator it, itlow, itup;

	itlow = dictionary.lower_bound(*word);
	itup = dictionary.upper_bound(*word);

	// todo remove one ocurence

}

void removeAllTranslates(std::string *word) {
	// todo remove multiple occurences
}

std::vector<std::string> findTranslate(std::string *word) {
	std::multimap<std::string, std::string>::iterator it, itlow, itup;
	std::vector<std::string> toReturn;

	itlow = dictionary.lower_bound(*word);
	itup = dictionary.upper_bound(*word);

	for (auto i = itlow; i != itup; i++)
	{
		toReturn.push_back((*i).second);
	}

	return toReturn;
}
