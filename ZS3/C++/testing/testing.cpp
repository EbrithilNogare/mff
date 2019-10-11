#include <iostream>
#include <string>
#include <vector>

std::string GetLine();
int CountWords(std::string data);
int CountChars(std::string data);
int CountLines(std::string data);
int CountSentences(std::string data);

int main()
{
	std::string data = GetLine();
	std::cout << "pocet slov: " << CountWords(data) << "\n";
	std::cout << "pocet znaku: " << CountChars(data) << "\n";
	std::cout << "pocet radku: " << CountLines(data) << "\n";
	std::cout << "pocet vet: " << CountSentences(data) << "\n";
	
	return 0;
}

std::string GetLine() {
	std::string data;
	std::getline(std::cin, data);
	return data;
}

int CountWords(std::string data) {
	int count = 0;
	char lastChar = '\0';
	for (size_t i = 0; i < data.length(); i++)
	{
		char separators[]{ ' ', '.', '!', '?' };
		if (std::find(std::begin(separators), std::end(separators), data[i]) != std::end(separators)) {
			if (std::find(std::begin(separators), std::end(separators), lastChar) == std::end(separators))
				count++;
		}
		lastChar = data[i];
	}
	return count;
}

int CountSentences(std::string data) {
	int count = 0;
	char lastChar = '\0';
	for (size_t i = 0; i < data.length(); i++)
	{
		char separators[]{ '.', '!', '?' };
		if (std::find(
				std::begin(separators),
				std::end(separators),
				data[i]
			)!= std::end(separators)) {
			if (std::find(
					std::begin(separators),
					std::end(separators),
					lastChar
				) == std::end(separators))
				count++;
		}
		lastChar = data[i];
	}
	return count;
}

int CountChars(std::string data) {
	int count = 0;
	for (size_t i = 0; i < data.length(); i++)
	{
		count++;
	}
	return count;
}

int CountLines(std::string data) {
	int count = 0;
	for (size_t i = 0; i < data.length(); i++)
	{
		if (data[i] == '\n')
			count++;
	}
	return count + 1;
}
