// polysort.hpp
// Author: David Napravnik

#define DEBUG

#ifndef polysort_H_
#define polysort_H_

#include <array>
#include <algorithm>
#include <iostream>
#include <string>
#include <map>
#include <fstream>
#include <vector>
#include "PolyContainer.hpp"
#include "IntVal.hpp"
#include "StringVal.hpp"


int main(int argc, char* argv[]);
bool parseParams(int argc, char** argv, std::string& paramI, std::string& paramO, char& paramS, std::vector<std::string>& paramRules);
void addRecord(std::vector<PolyContainer>& container, std::string& data, char separator, std::map<int, char> dataTypes);


#endif // polysort_H_