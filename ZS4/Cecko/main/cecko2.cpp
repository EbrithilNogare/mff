/*

cecko2.cpp

main program

*/

#include "ckmain.hpp"

#include "caparser.hpp"

int main(int argc, char **argv)
{
	cecko::main_state_code ms;

	auto rv1 = ms.setup(argc, argv);
	if (!rv1)
		return 0;

	auto rv2 = ms.parse< cecko::parser>();
	if (!rv2)
		return 0;

	std::cout << "========== cecko2 done ==========" << std::endl;

	return 0;
}
