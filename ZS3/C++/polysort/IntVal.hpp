// IntVal.hpp
// Author: David Napravnik

#include "AbstractVal.hpp"

#ifndef IntVal_H_
#define IntVal_H_

class IntVal : public AbstractVal {
private:
	int x_;
public:
	IntVal(int x) { x_ = x; }
	virtual void print(std::ostream& outStream) { outStream << x_; };
};

#endif // IntVal_H_