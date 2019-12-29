#include <iostream>
int main(int argc, char* args[])
{
	for (int i = 0; i < argc; ++i)
	{
		std::cout << args[i] << " ";
	}
	return 0;
}