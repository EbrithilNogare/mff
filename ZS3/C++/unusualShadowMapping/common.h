#pragma once

#include <string>
#include <vector>

std::vector<std::string> SplitString(std::string &input, char delimiter) {
	std::vector<std::string> toReturn;
	size_t oldPos = 0;
	size_t pos = 0;
	while ((pos = input.find(delimiter, oldPos)) != std::string::npos) {
		toReturn.push_back(input.substr(oldPos, pos - oldPos));
		oldPos = pos+1;
	}
	toReturn.push_back(input.substr(oldPos));
	return toReturn;
}