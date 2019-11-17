// automat.h
// author : David Napravnik

#include <iostream>
#include <vector>
#include <set>
#include <algorithm>
#include <map>

using namespace std;

class Automat{
public:
	enum symbol { number, letter, fence, space, other }; // [0-9], [a-zA-z], [#], [ ]
	enum state { s0, s1, s2, s3, s4, s5, s6, er };

	map<string, string> marcoList;
	vector<char> wordStorage;
	vector<char> macroStorage;
	state currentState;
	vector<vector<state>> automatDirections;

	Automat();

	symbol GetSymbolType(char inputSymbol);

	void runAutomat(char inputSymbol);

	void state0(char input);
	void state1(char input);
	void state2(char input);
	void state3(char input);
	void state4(char input);
	void state5(char input);
	void state6(char input);
	void stateEr(char input);
};