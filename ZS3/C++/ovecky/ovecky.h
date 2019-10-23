#include <string>
#include <vector>

class ovecky {
public:
	std::vector<int> getNumbers(std::string data);
	int CountWords(std::string data);
	int CountChars(std::string data);
	int CountLines(std::string data);
	int CountSentences(std::string data);
	int CountNumbers(std::string data);
	int SumOfNumbers(std::string data);
};