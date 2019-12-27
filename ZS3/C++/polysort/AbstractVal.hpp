// AbstractVal.hpp
// Author: David Napravnik

#ifndef AbstractVal_H_
#define AbstractVal_H_

#include <iostream>

class AbstractVal {
private:
public:
	virtual void print(std::ostream& outStream) {};
	virtual bool operator > (AbstractVal const& obj) { return false; };
	virtual bool operator == (AbstractVal const& obj) { return false; };
};



#endif // AbstractVal_H_