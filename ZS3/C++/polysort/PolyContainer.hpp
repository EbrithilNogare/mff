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
private:
	std::vector<std::shared_ptr<AbstractVal>> container_; // todo fix: shared -> unique
public:
	void add(std::unique_ptr<AbstractVal> p) { container_.push_back(std::move(p)); }
	void print(std::ostream& s, char separator); 
	std::shared_ptr<AbstractVal> getOnIndex(int index) { return container_.at(index); }
};






#endif // PolyContainer_H_