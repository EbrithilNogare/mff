// StringVal.hpp
// Author: David Napravnik

#ifndef StringVal_HPP_
#define StringVal_HPP_

#include <string>
#include "AbstractVal.hpp"

class StringVal : public AbstractVal {
private:
	std::string x_;
public:
	StringVal(std::string x) { x_ = x; }
	virtual void print(std::ostream& outStream) { outStream << x_; };
	virtual bool operator > (StringVal const& obj) { return obj.x_ > x_; };
	virtual bool operator == (StringVal const& obj) { return obj.x_ == x_; };
};

#endif // StringVal_H_