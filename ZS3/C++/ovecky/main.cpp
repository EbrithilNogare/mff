#include "main.h"


int main()
{
	std::string data = GetLine();
	ovecky o;
	std::cout << "znaku: " << o.CountChars(data) << "\n";
	std::cout << "slov: " << o.CountWords(data) << "\n";
	std::cout << "vet: " << o.CountSentences(data) << "\n";
	std::cout << "radku: " << o.CountLines(data) << "\n";
	std::cout << "cisel: " << o.CountNumbers(data) << "\n";
	std::cout << "soucet: " << o.SumOfNumbers(data) << "\n";

	return 0;
}

std::string GetLine() {
	std::string allData;
	std::string data;
	while (! std::cin.eof())
	{
		std::getline(std::cin, data);
		allData += data + '\n';
	}
	return allData;
}
