// automat.cpp
// author : David Napravnik

#include <iostream>
#include <vector>
#include <set>
#include <algorithm>
#include "automat.h"

using namespace std;

enum symbol {number, letter, fence, space}; // [0-9], [a-zA-z], [#], [ ]
enum state {s1, s2, s3, s4, s5, s6, err};
state currentState = s1;

void automat(char inputSymbol) {
	symbol symbolType;
	if (isdigit(inputSymbol))symbolType = number;
	else if (isalpha(inputSymbol))symbolType = letter;
	else if (inputSymbol=='#')symbolType = fence;
	else if (isspace(inputSymbol))symbolType = space;
	// todo else some error

	state newState;
	switch (currentState)
	{
	s1:newState = state1(symbolType); break;
	s2:newState = state2(symbolType); break;
	s3:newState = state3(symbolType); break;
	s4:newState = state4(symbolType); break;
	s5:newState = state5(symbolType); break;
	s6:newState = state6(symbolType); break;
	}

	currentState = newState;
}

state state1(symbol input) { // plain text
	switch (input) {
	number:
	letter:
	hash: return s1; break;
	space: return s2; break;
	}
}
state state2(symbol input) { // end of word
	switch (input) {
	number:
	letter: return s1; break;
	hash: return s3; break;
	space: return s2; break;
	}
}
state state3(symbol input) { // first letter of macro name
	switch (input) {
	letter: return s4; break;
	number:
	hash:
	space: return err; break;
	}
}
state state4(symbol input) { // inside of macro name
	switch (input) {
	number:
	letter:return s4; break;
	hash: return err; break;
	space: return s5; break;
	}
}
state state5(symbol input) { // inside of macro content
	switch (input) {
	number:
	letter:
	space: return s5; break;
	hash: return s6; break;
	}
}
state state6(symbol input) { // end of macro
	switch (input) {
	number:
	letter:
	hash: return err; break;
	space: return s2; break;
	}
}



