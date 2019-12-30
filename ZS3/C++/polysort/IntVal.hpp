// IntVal.hpp
// Author: David Napravnik

#ifndef IntVal_HPP_
#define IntVal_HPP_

#include "AbstractVal.hpp"

class IntVal : public AbstractVal {
private:
	int x_;
public:
	IntVal(int x) { x_ = x; }
	virtual void print(std::ostream& outStream) { outStream << x_; };
	bool operator > (IntVal const& obj) { return obj.x_ > x_; };
	bool operator == (IntVal const& obj) { return obj.x_ == x_; };
};

#endif // IntVal_H_