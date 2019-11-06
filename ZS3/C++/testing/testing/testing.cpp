// File: testing.cpp
// Author: David Napravnik

#include <iostream>
#include <string>
#include <vector>
#include <map>
#include <algorithm>


std::vector<std::pair<std::string, int>> database;

void addFilm(std::string name, int date) {
	database.push_back(std::pair<std::string, int>{name, date});
}

bool mySort(const std::pair<std::string, int>& s1, const std::pair<std::string, int>& s2) {
	
	if (s1.first < s2.first)
		return true;
	else if (s1.first > s2.first)
		return false;
	else
		return (s1.second < s2.second);
}

void sortDatabase() {
	std::sort(database.begin(), database.end(), mySort);
}

class std::pair<std::string, int> {
	bool operator<(const std::pair<std::string, int>& y) {
		if (this.first < y.first)
			return true;
		else if (this.first > y.first)
			return false;
		else
			return (this.second< y.second);
	}
};

void showDatabase() {
	for (auto&& i : database)
	{	
		std::cout << i.first << ": "<< i.second << std::endl;
	}
}


int main()
{

	addFilm("nazev1", 1021);
	addFilm("nazev4", 1014);
	addFilm("nazev2", 1092);
	addFilm("nazev3", 1043);
	addFilm("nazev3", 1045);
	addFilm("nazev3", 1044);

	sortDatabase();

	showDatabase();
}


/*	slovnik
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
*/
