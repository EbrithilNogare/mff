#pragma once

#include <iostream>
#include <array>
#include <vector>

using namespace std;

class GummyArray {
private:
	static const int blockSize = 8; // todo: increase this value
	int pointer = 0;
public:
	
	void push_back(int item) {
		if (pointer % blockSize == 0) { // create new block
			array<int, 8> arrs = { 0, 0, 0, 0, 0, 0, 0, 0};
		}
		//library[pointer / blockSize][pointer % blockSize] = item;
		pointer++;
	}
	/*
	typedef library* iterator;
	typedef const library* const_iterator;
	iterator begin() { return 0; }
	iterator end() { return pointer; }*/
};