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

	// get params
	if (!parseParams(argc, argv, paramI, paramO, paramS, paramRules)) {
		cerr << "invalid set of parameters";
		return 1;
	}

#ifdef DEBUG
	cout << "paramI: " << paramI << "\n" <<
		"paramO: " << paramO << "\n" <<
		"paramS: " << paramS << "\n";
	for (const auto& rule : paramRules)
		cout << "paramRules: " << rule << "\n";
#endif // DEBUG


	// set input
	istream& inputStream = paramI.empty() ? cin : *(new ifstream(paramI));


	// set output
	ostream& outputStream = paramO.empty() ? cout : *(new ofstream(paramO));


	// create container



	// fill container
	string line;
	while (std::getline(inputStream, line))
	{
		outputStream << line << "\n";
	}


	// sort container



	// return result



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
				if (*++argv)
					paramI = *argv;
				else
					return false;
				break;
			case 'o':
				if (*++argv)
					paramO = *argv;
				else
					return false;
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
