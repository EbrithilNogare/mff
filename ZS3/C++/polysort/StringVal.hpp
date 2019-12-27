// StringVal.hpp
// Author: David Napravnik

#ifndef StringVal_H_
#define StringVal_H_

#include <string>
#include "AbstractVal.hpp"

class StringVal : public AbstractVal {
private:
	std::string x_;
public:
	StringVal(std::string x) { x_ = x; }
	virtual void print(std::ostream& outStream) { outStream << x_; };
	virtual bool operator > (AbstractVal const& obj) { return true; }; // todo
	virtual bool operator == (AbstractVal const& obj) { return false; }; // todo
};

#endif // StringVal_H_