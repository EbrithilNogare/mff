/*

cecko4.cpp

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

	auto rv3 = ms.dump_tables();
	if (!rv3)
		return 0;

	auto rv4 = ms.dump_code();
	if (!rv4)
		return 0;

	auto rv5 = ms.run_code();
	if (!rv5)
		return 0;

	std::cout << "========== cecko4 done ==========" << std::endl;

	return 0;
}
