#include <iostream>
#include <string>
#include <sstream>
#include <vector>
#include <algorithm>
#include "ovecky.h"

int ovecky::CountWords(std::string data) {
	int count = 0;
	std::string word;
	std::stringstream iss(data);
	while (iss >> word)
		if(!isdigit(word[0]))
			count++;
	return count;
}

int ovecky::CountSentences(std::string data) {
	int count = 0;
	char lastChar = '\0';
	for (size_t i = 0; i < data.length(); i++)
	{
		char separators[]{ '.', '!', '?' };
		if (std::find(
			std::begin(separators),
			std::end(separators),
			data[i]
		) != std::end(separators)) {
			if (std::find(
				std::begin(separators),
				std::end(separators),
				lastChar
			) == std::end(separators))
				count++;
		}
		lastChar = data[i];
	}
	return count;
}

int ovecky::CountChars(std::string data) {
	int count = 0;
	for (size_t i = 0; i < data.length(); i++)
	{
		count++;
	}
	return count;
}

int ovecky::CountLines(std::string data) {
	int count = 0;
	for (size_t i = 0; i < data.length(); i++)
	{
		if (data[i] == '\n')
			count++;
	}
	return count + 1;
}

int ovecky::CountNumbers(std::string data) {
	return (int)getNumbers(data).size();
}

int ovecky::SumOfNumbers(std::string data) {
	int count = 0;
	std::vector<int> numbers = getNumbers(data);
	for (std::vector<int>::iterator it = numbers.begin(); it != numbers.end(); ++it)
		count += *it;
	return count;
}

std::vector<int> ovecky::getNumbers(std::string data) {
	std::vector<int> output;
	std::stringstream ss;
	ss << data;

	std::string temp;
	int found;
	while (!ss.eof()) {
		ss >> temp;
		if (std::stringstream(temp) >> found)
			output.push_back(found);
		temp = "";
	}
	return output;

}