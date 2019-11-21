// automat.cpp
// author : David Napravnik

#include "automat.h"

//#define debug

Automat::Automat(void)
{
	Automat::currentState = s0;
	Automat::automatDescription = {
		{s0, s0, s0, s1, s0}, //s0
		{s0, s0, s2, s1, s0}, //s1
		{er, s3, er, er, er}, //s2
		{s3, s3, er, s4, er}, //s3
		{s4, s4, er, s5, s4}, //s4
		{s4, s4, s6, s5, s4}, //s5
		{er, er, er, s1, er}, //s6
		{er, er, er, er, er}, //er
	};
}

void Automat::addMacro(vector<string> macro){
	currentState = Automat::s2;
	for (int i = 1; i < macro.size(); i++)
	{
		string word = macro[i];
		for (int j = 0; j < word.length(); j++)
		{
			runAutomat(word[j]);
		}
		runAutomat(' ');
	}
	runAutomat('#');
	runAutomat(' ');
}
void Automat::runAutomat(char inputSymbol) {
	symbol symbolType = GetSymbolType(inputSymbol);

	currentState = Automat::automatDescription[Automat::currentState][symbolType];

	switch (currentState) {
	case s0: state0(inputSymbol); break;
	case s1: state1(inputSymbol); break;
	case s2: state2(inputSymbol); break;
	case s3: state3(inputSymbol); break;
	case s4: state4(inputSymbol); break;
	case s5: state5(inputSymbol); break;
	case s6: state6(inputSymbol); break;
	default: stateEr(inputSymbol);
	}

#ifdef debug
	cout << "input symbol: " << inputSymbol << "     automat state: " << currentState << " \033[" << (
		currentState < 2 ? 42 :
		currentState < 4 ? 43 :
		currentState < 7 ? 44 : 41
		) << "m" << string(currentState, ' ') << " \033[0m.\n";
#endif
}

Automat::symbol Automat::GetSymbolType(char inputSymbol)
{
	if (isdigit(inputSymbol))return number;
	if (isalpha(inputSymbol))return letter;
	if (inputSymbol == '#')return fence;
	if (isspace(inputSymbol))return space;
	else return other;
}

void Automat::state0(char input) {
	wordStorage.push_back(input);
}
void Automat::state1(char input) {
	// check if it is macro name or plain text
	string word(wordStorage.begin(), wordStorage.end());
	wordStorage.clear();

	if (marcoList.count(word) > 0) {
		string macro = marcoList[word];
		for (int i = 0; i < macro.length(); i++)
		{
			runAutomat(macro[i]);
		}
	}
	else {
		cout << word << input;
	}
}
void Automat::state2(char input) {

}
void Automat::state3(char input) {
	wordStorage.push_back(input);
}
void Automat::state4(char input) {
	macroStorage.push_back(input);
}
void Automat::state5(char input) {
	macroStorage.push_back(input);
}
void Automat::state6(char input) {
	marcoList.insert(pair<string, string>(
		string(wordStorage.begin(), wordStorage.end()),
		string(macroStorage.begin(), macroStorage.end())
		));
	wordStorage.clear();
	macroStorage.clear();
}
void Automat::stateEr(char input) {
	return;
}


