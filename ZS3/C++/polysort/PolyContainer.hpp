// PolyContainer.hpp
// Author: David Napravnik

#ifndef PolyContainer_HPP_
#define PolyContainer_HPP_

#include <memory>
#include <vector>

#include "AbstractVal.hpp"

using valptr = std::shared_ptr<AbstractVal>;

class PolyContainer
{
private:
	std::vector<valptr> container_; // todo fix: shared -> unique
public:
	void add(std::shared_ptr<AbstractVal> p) { container_.push_back(std::move(p)); }
	void print(std::ostream& s, char separator);
	std::shared_ptr<AbstractVal> getOnIndex(int index) { return container_.at(index); }
};

#endif // PolyContainer_H_