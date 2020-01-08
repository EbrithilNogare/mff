#pragma once

#include <vector>
#include <string>
#include <exception>

class ownContainer
{
private:
	std::vector<int> container_;
	size_t maxSize;
public:
	ownContainer(size_t maxSize) {
		container_ = std::vector<int>();
		this->maxSize = maxSize;
	}
	void add(int item) {
		if (maxSize <= container_.size())
			throw std::length_error("too large");

		container_.push_back(item);
	}
	int operator [](int index) {
		if (index >= container_.size())
			throw std::out_of_range("bad index");

		return container_[index];
	}
};

