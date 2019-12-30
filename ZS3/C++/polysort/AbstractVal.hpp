// AbstractVal.hpp
// Author: David Napravnik
//
// Abstract data structure for PolyContainer

#ifndef AbstractVal_HPP_
#define AbstractVal_HPP_

#include <iostream>

class AbstractVal {
private:
public:
	virtual void print(std::ostream& outStream) {};
	virtual bool operator > (AbstractVal const& obj) { throw "using abstract method"; };
	virtual bool operator == (AbstractVal const& obj) { throw "using abstract method"; };
};

#endif // AbstractVal_H_