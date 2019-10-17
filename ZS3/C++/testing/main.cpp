#include <iostream>
#include <string>
#include <vector>
#include "main.h"
#include "ovecky.h"


int main()
{
	std::string data = GetLine();
	
	ovecky o;

	std::cout << "znaku: " << o.CountChars(data) << "\n";
	std::cout << "slov: " << o.CountWords(data) << "\n";
	std::cout << "vet: " << o.CountSentences(data) << "\n";
	std::cout << "radku: " << o.CountLines(data) << "\n";
	std::cout << "pocet: " << o.CountNumbers(data) << "\n";
	std::cout << "soucet: " << o.SumOfNumbers(data) << "\n";
	
	return 0;
}

std::string GetLine() {
	std::string data;
	std::getline(std::cin, data);
	return data;
}