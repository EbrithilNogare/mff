#include <string>
#include <vector>
class ovecky
{
	private:
		std::vector<int> getNumbers(std::string data);
	public:
		int CountWords(std::string data);
		int CountChars(std::string data);
		int CountLines(std::string data);
		int CountSentences(std::string data);
		int CountNumbers(std::string data);
		int SumOfNumbers(std::string data);
};

