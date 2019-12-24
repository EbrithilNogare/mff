// polysort.cpp
// Author: David Napravnik

#include "polysort.hpp"

using namespace std;

int main(int argc, char* argv[])
{
	// init
	string paramI;
	string paramO;
	char paramS = ' ';
	vector<string> paramRules;
	vector<PolyContainer> container;

	// get params
	if (!parseParams(argc, argv, paramI, paramO, paramS, paramRules)) {
		cerr << "error: invalid set of parameters";
		return 0;
	}

	// configure data types
	map<int, char> dataTypes;
	for (string const& value : paramRules)
		dataTypes.insert(pair<int, char>(stoi(value.substr(1)), value[0]));
	
	
	// set input
	istream& inputStream = paramI.empty() ? cin : *(new ifstream(paramI));


	// set output
	ostream& outputStream = paramO.empty() ? cout : *(new ofstream(paramO));


	// fill container
	string line;
	while (std::getline(inputStream, line))
		addRecord(container, line, paramS, dataTypes);


	// sort container
	//sort(container.begin(), container.end(), polyComparer); // todo continue here
	


	// return result
	for (auto& item : container) {
		item.print(outputStream, paramS);
		outputStream << "\n";
	}


}

bool parseParams(int argc, char** argv, string& paramI, string& paramO, char& paramS, vector<string>& paramRules) {
	if (argc == 1) {
		cout <<
			"usage: polysort [-i in] [-o out] [-s separator] { type colnum }\n" <<
			"- i: vstupni soubor(default: stdin)\n" <<
			"- o : vystupni soubor(default: stdout)\n" <<
			"- s : separator(default: ' ')\n" <<
			"type : S - string, N - numeric, ...\n" <<
			"column : cislo logického sloupce(pocitano od 1)\n";
		return false;
	}
	while (*++argv) {
		if (**argv == '-') {
			switch (argv[0][1]) {
			case 'i':
				if (argv[0][2])
					paramI = *argv + 2;
				else {
					if (*++argv)
						paramI = *argv;
					else
						return false;
				}
				break;
			case 'o':
				if (argv[0][2])
					paramO = *argv+2;
				else {
					if (*++argv)
						paramO = *argv;
					else
						return false;
				}
				break;
			case 's':
				if (argv[0][2])
					paramS = argv[0][2];
				else {
					if (*++argv)
						paramS = **argv;
					else
						return false;
				}
				break;
			}
		}
		else {
			paramRules.push_back(*argv);
		}
	}
	return true;
}

void addRecord(vector<PolyContainer>& container, string& data, char separator, map<int, char> dataTypes)
{
	int fieldIndex = 0;
	vector<char> buffer;
	PolyContainer newItem;

	for (auto it = data.begin(); it != data.end(); it++) {
		if (*it != separator) {
			buffer.push_back(*it);
			if(next(it) != data.end())
				continue;
		}

		fieldIndex++;
		char currentType = 'S';
		if (dataTypes.find(fieldIndex) != dataTypes.end())
			currentType = dataTypes.at(fieldIndex);

		string tempString = string(buffer.begin(), buffer.end());
		switch (currentType)
		{
		case 'S':
			newItem.add(make_unique<StringVal>(tempString));
			break;
		case 'N':
			newItem.add(make_unique<IntVal>(stoi(tempString))); // todo add some protection against non number
			break;
		default:
			throw "unsupported data type";
		}
			
		buffer.clear();
	}
	container.push_back(newItem);
}
