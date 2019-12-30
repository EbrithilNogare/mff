// polysort.hpp
// Author: David Napravnik

#ifndef polysort_HPP_
#define polysort_HPP_

#include <algorithm>
#include <array>
#include <fstream>
#include <iostream>
#include <map>
#include <string>
#include <vector>

#include "AbstractVal.hpp"
#include "IntVal.hpp"
#include "PolyContainer.hpp"
#include "StringVal.hpp"

int main(int argc, char* argv[]);
bool parseParams(int argc, char** argv, std::string& paramI, std::string& paramO, char& paramS, std::vector<std::string>& paramRules);
bool addRecord(std::vector<PolyContainer>& container, std::string& data, char separator, std::map<int, char> dataTypes);

#endif // polysort_H_