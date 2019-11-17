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

	// todo add macros in parameters support
	vector<string> arg(argv, argv + argc);
	if (arg.size() > 1) {
		automat.currentState = Automat::s2;
		for (int i = 1; i < arg.size(); i++)
		{
			string word = arg[i];
			for (int j = 0; j < word.length(); j++)
			{
				automat.runAutomat(word[j]);
			}
			automat.runAutomat(' ');
		}
		automat.runAutomat('#');
		automat.runAutomat(' ');
	}

	istream& s = cin;
	char c;
	for (;;)
	{
		c = s.get();
		if (s.fail()) break;
		//cout << (c == '\n' ? '^':c) ;
		automat.runAutomat(c);
	}
	if(automat.currentState == Automat::er)
		cout << "Error" << "\n";
}



