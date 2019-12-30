// polysort.cpp
// Author: David Napravnik

#include "polysort.hpp"

int main(int argc, char* argv[])
{
	// init
	std::string paramI;
	std::string paramO;
	char paramS = ' ';
	std::vector<std::string> paramRules;
	std::vector<PolyContainer> container;
	std::map<int, char> dataTypes;
	const char sdt[] = {'S', 'N'}; // supportedDataTypes

	// get params
	if (!parseParams(argc, argv, paramI, paramO, paramS, paramRules)) {
		std::cerr << "error: invalid set of parameters" << std::endl;
		return 0;
	}

	// configure data types
	for (std::string const& value : paramRules) {
		if (std::find(sdt, sdt + std::size(sdt), value[0]) == sdt + std::size(sdt)) {
			std::cerr << "unsupported data type" << std::endl;
			return 0;
		}
		dataTypes.insert(std::pair<int, char>(stoi(value.substr(1)), value[0]));
	}
	
	
	// set input
	std::istream& inputStream = paramI.empty() ? std::cin : *(new std::ifstream(paramI));


	// set output
	std::ostream& outputStream = paramO.empty() ? std::cout : *(new std::ofstream(paramO));


	// fill container	
	for (std::string line; std::getline(inputStream, line);) {
		if (!addRecord(container, line, paramS, dataTypes)) {
			std::cerr << "type mismatch in: " << line << std::endl;
			return 0;
		}
	}

	// sort container
	auto comp = [paramRules](PolyContainer a, PolyContainer b) {
		for (auto const& value : paramRules)
		{
			const int index = stoi(value.substr(1)) - 1; // base change from one to zero

			auto absoluteA = a.getOnIndex(index);
			auto absoluteB = b.getOnIndex(index);

			switch (value[0]) {
			case 'S': {
				StringVal& a = dynamic_cast<StringVal&>(*absoluteA);
				StringVal& b = dynamic_cast<StringVal&>(*absoluteB);
				if (a == b) continue;
				return a > b; 
			}
			case 'N': {
				IntVal& a = dynamic_cast<IntVal&>(*absoluteA);
				IntVal& b = dynamic_cast<IntVal&>(*absoluteB);
				if (a == b) continue;
				return a > b;
			}
			}
		}
		return false;
	};
	std::sort(container.begin(), container.end(), comp);
	

	// return result
	for (auto& item : container) {
		item.print(outputStream, paramS);
		outputStream << "\n";
	}
}

bool parseParams(int argc, char** argv, std::string& paramI, std::string& paramO, char& paramS, std::vector<std::string>& paramRules) {
	if (argc == 1) {
		std::cout <<
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

/// <summary>
/// <returns>false if sometin went wrong</returns>
/// </summary>
bool addRecord(std::vector<PolyContainer>& container, std::string& data, char separator, std::map<int, char> dataTypes)
{
	int fieldIndex = 0;
	std::vector<char> buffer;
	PolyContainer newItem;

	data.append(&separator);
	for (auto it = data.begin(); it != data.end(); it++) {
		if (*it != separator) {
			buffer.push_back(*it);
			continue;
		}

		fieldIndex++;
		char currentType = 'S';
		if (dataTypes.find(fieldIndex) != dataTypes.end())
			currentType = dataTypes.at(fieldIndex);

		std::string tempString = std::string(buffer.begin(), buffer.end());
		switch (currentType)
		{
		case 'S':
			newItem.add(std::make_shared<StringVal>(tempString));
			break;
		case 'N': {
			try {
				newItem.add(std::make_shared<IntVal>(stoi(tempString)));
			}
			catch (std::invalid_argument) { // catch error from std::stoi
				return false;
			}
			break;
		}			
		}
			
		buffer.clear();
	}
	container.push_back(newItem);
	return true;
}
