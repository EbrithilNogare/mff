/**
 * @file ovecky.cpp
 *
 * @author David Napravnik
 * Contact: d@nogare.cz
 */
#include "ovecky.h"

int ovecky::ComputeAll(std::istream& input) {
	char c;

	while (true) {
		c = input.get();
		if (input.fail())
			return 0;

		if (isalpha(c))
			currentTransition = transition::letter;
		else if (isalnum(c))
			currentTransition = transition::number;
		else
			currentTransition = transition::other;

		chars++;
		CountWords(c);
		CountSentences(c);
		CountLines(c);
		CountNumbers(c);
	}

	return 0;
}

void ovecky::CountWords(char data) {
	switch (stateWords) {
	case 0: // in word
		switch (currentTransition) {
		case transition::letter: stateWords = 1; words++; break;
		case transition::number: stateWords = 0; break;
		case transition::other: stateWords = 0; break;
		} break;
	case 1: // not in word
		switch (currentTransition) {
		case transition::letter: stateWords = 1; break;
		case transition::number: stateWords = 1; break;
		case transition::other: stateWords = 0; break;
		} break;
	}
}

void ovecky::CountSentences(char data) {
	enum sentenseTransition { letter, mark, other };
	sentenseTransition currentSentenseTransition;

	if (data == '.' || data == '?' || data == '!')
		currentSentenseTransition = mark;
	else if (currentTransition == transition::letter)
		currentSentenseTransition = sentenseTransition::letter;
	else
		currentSentenseTransition = sentenseTransition::other;

	switch (stateSentences) {
	case 0: // outside of sentense
		switch (currentSentenseTransition) {
		case sentenseTransition::letter: stateSentences = 1; break;
		case sentenseTransition::mark: stateSentences = 0; break;
		case sentenseTransition::other: stateSentences = 0; break;
		} break;
	case 1: // inside of sentense
		switch (currentSentenseTransition) {
		case sentenseTransition::letter: stateSentences = 1; break;
		case sentenseTransition::mark: stateSentences = 0; sentences++; break;
		case sentenseTransition::other: stateSentences = 1; break;
		} break;
	}
}

void ovecky::CountLines(char data) {
	//BUG empty lines are also taken
	if (data == '\n')
		lines++;
}

void ovecky::CountNumbers(char data) {
	switch (stateNumbers) {
	case 0: // outside
		switch (currentTransition) {
		case transition::letter: stateNumbers = 2; break;
		case transition::number: stateNumbers = 1; numbers++; currentNumber += data; break;
		case transition::other: stateNumbers = 0; break;
		} break;
	case 1: // inside number
		switch (currentTransition) {
		case transition::number: stateNumbers = 1; currentNumber += data; break;
		case transition::letter:
		case transition::other: stateNumbers = 0;
			sum += std::stoi(currentNumber);
			currentNumber = "";
			break;
		} break;
	case 2: // inside word
		switch (currentTransition) {
		case transition::letter: stateNumbers = 2; break;
		case transition::number: stateNumbers = 2; break;
		case transition::other: stateNumbers = 0; break;
		} break;
	}
}