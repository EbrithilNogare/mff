/**
 * @file main.cpp
 *
 * @author David Napravnik
 * Contact: d@nogare.cz
 */
#include "main.h"


int main()
{
	ovecky o;

	o.ComputeAll(std::cin);

	std::cout << "znaku: "	<< o.chars		<< "\n";
	std::cout << "slov: "	<< o.words		<< "\n";
	std::cout << "vet: "	<< o.sentences	<< "\n";
	std::cout << "radku: "	<< o.lines		<< "\n";
	std::cout << "cisel: "	<< o.numbers	<< "\n";
	std::cout << "soucet: " << o.sum		<< "\n";

	return 0;
}