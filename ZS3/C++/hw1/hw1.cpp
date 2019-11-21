// hw1.cpp : This file contains the 'main' function. Program execution begins and ends there.
// author : David Napravnik

#include <iostream>
#include <vector>
#include <set>
#include <algorithm>
#include "automat.h"

using namespace std;

int main(int argc, char** argv)
{
	Automat automat;

	vector<string> arg(argv, argv + argc);
	if (arg.size() > 1) {
		automat.addMacro(arg);
	}

	char c;
	for (;;)
	{
		c = cin.get();
		if (cin.fail()) break;

		automat.runAutomat(c);

		if (automat.currentState == Automat::er) {
			cout << "Error" << "\n";
			break;
		}
	}
}



