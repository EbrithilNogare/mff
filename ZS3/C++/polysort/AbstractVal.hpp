// AbstractVal.hpp
// Author: David Napravnik

#ifndef AbstractVal_H_
#define AbstractVal_H_

#include <iostream>

class AbstractVal {
public:
	virtual void print(std::ostream& outStream) {};
};



#endif // AbstractVal_H_