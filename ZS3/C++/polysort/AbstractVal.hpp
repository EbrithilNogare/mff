// AbstractVal.hpp
// Author: David Napravnik

#ifndef AbstractVal_H_
#define AbstractVal_H_

#include <iostream>

class AbstractVal {
private:
public:
	virtual void print(std::ostream& outStream) {};
	/*/switch
	virtual bool operator > (AbstractVal const& obj) = 0;
	virtual bool operator == (AbstractVal const& obj) = 0;
	/*/
	virtual bool operator > (AbstractVal const& obj) { return true; };
	virtual bool operator == (AbstractVal const& obj) { return true; };
	/**/
};



#endif // AbstractVal_H_