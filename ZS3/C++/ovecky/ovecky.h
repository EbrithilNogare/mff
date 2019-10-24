#include <string>
#include <vector>
#include <istream>

class ovecky {
private:
	int stateWords = 0;
	int stateSentences = 0;
	int stateNumbers = 0;
	enum transition { letter, number, other };
	transition currentTransition;
public:
	int chars = 0;
	int words = 0;
	int lines = 1;
	int sentences = 0;
	int numbers = 0;
	int sum = 0;

private:
public:
	int ComputeAll(std::istream& input);
	void CountWords(char data);
	void CountLines(char data);
	void CountSentences(char data);
	void CountNumbers(char data);
	std::string currentNumber;
};

