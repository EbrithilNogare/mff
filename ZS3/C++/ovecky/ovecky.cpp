#include "ovecky.h"

int ovecky::CountWords(std::string data) {
	int count = 0;
	bool inWord = false;
	bool inNumber = false;
	for (int i = 0; i < data.length(); i++)
	{
		if (!isalnum(data[i])) {
			if (inWord)
				count++;
			inWord = false;
			inNumber = false;
			continue;
		}

		if (inWord || inNumber)
			continue;

		if (isdigit(data[i])) {
			inNumber = true;
		}
		else {
			inWord = true;
		}
	}
	return count;
}

int ovecky::CountSentences(std::string data) {
	int count = 0;
	int state = 0;
	int character = 0; // 0=letter, 1=sentence separator, 2=other


	for (int i = 0; i < data.length(); i++)
	{
		//set direction
		if (isalnum(data[i]) && !isdigit(data[i]))
			character = 0;
		else if (data[i] == '.' || data[i] == '?' || data[i] == '!')
			character = 1;
		else
			character = 2;

		//step in automata
		switch (state)
		{
		case 0:
			switch (character)
			{
			case 0:
				state = 1;
				break;
			case 1:
			case 2:
				state = 0;
				break;
			}
			break;
		case 1:
			switch (character)
			{
			case 0:
			case 2:
				state = 1;
				break;
			case 1:
				state = 0;
				count++;
				break;
			}
			break;
		}
	}
	return count;

}

int ovecky::CountChars(std::string data) {
	int count = 0;
	for (size_t i = 0; i < data.length(); i++)
	{
		count++;
	}
	return count - 1;
}

int ovecky::CountLines(std::string data) {
	int count = 0;
	for (size_t i = 0; i < data.length(); i++)
	{
		if (data[i] == '\n')
			count++;
	}
	return count;
}

int ovecky::CountNumbers(std::string data) {
	return (int)getNumbers(data).size();
}

int ovecky::SumOfNumbers(std::string data) {
	int count = 0;
	std::vector<int> numbers = getNumbers(data);
	for (std::vector<int>::iterator it = numbers.begin(); it != numbers.end(); ++it)
		count += *it;
	return count;
}

std::vector<int> ovecky::getNumbers(std::string data) {
	std::vector<int> output;
	int state = 0;
	int character = 0; // 0=number, 1=letter, 2=other
	std::string uncompleteNumber = "";


	for (int i = 0; i < data.length(); i++)
	{
		//set direction
		if (isdigit(data[i]))
			character = 0;
		else if (isalnum(data[i]))
			character = 1;
		else
			character = 2;

		//step in automata
		switch (state)
		{
		case 0:
			switch (character)
			{
			case 0:
				state = 1;
				uncompleteNumber += data[i];
				break;
			case 1:
				state = 2;
				break;
			case 2:
				state = 0;
				break;
			}
			break;
		case 1:
			switch (character)
			{
			case 0:
				state = 1;
				uncompleteNumber += data[i];
				break;
			case 1:
			case 2:
				state = 0;
				output.push_back(std::stoi(uncompleteNumber));
				uncompleteNumber = "";
				break;
			}
			break;
		case 2:
			switch (character)
			{
			case 0:
			case 1:
				state = 2;
				break;
			case 2:
				state = 0;
				break;
			}
			break;
		}
	}
	return output;

}