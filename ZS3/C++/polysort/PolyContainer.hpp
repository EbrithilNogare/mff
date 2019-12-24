// PolyContainer.hpp
// Author: David Napravnik

#ifndef PolyContainer_H_
#define PolyContainer_H_

#include <string>
#include <array>
#include <vector>
#include <map>
#include <memory>
#include "AbstractVal.hpp"

class PolyContainer
{
public:
	void add(std::unique_ptr<AbstractVal> p) { container_.push_back(std::move(p)); }
	void print(std::ostream& s, char separator);

private:
	std::vector<std::shared_ptr<AbstractVal>> container_; // todo fix: shared -> unique
};






#endif // PolyContainer_H_