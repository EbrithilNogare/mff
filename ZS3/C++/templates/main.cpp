// main.cpp
// author: David Napravnik

#include <string>
#include <vector>
#include <iostream>

#include "Zlomek.h"

using namespace std;

int main() {

	Zlomek<float> a{ 1,2 };
	Zlomek<float> b{ 1,2 };
	Zlomek<float> c = a + b;
	cout << c.result() <<"\n";


}