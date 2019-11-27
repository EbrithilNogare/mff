// Zlomek.h
// author: David Napravnik

#include <string>

using namespace std;

template<typename T>class Zlomek
{
public:
	Zlomek(T x, T y) {
		valUp = x;
		valDown = y;
	};
	template<typename T>friend Zlomek<T> operator+(const Zlomek<T>& x, const Zlomek<T>& y);
	string result();
private:
	T valUp;
	T valDown;
};


template<typename T> Zlomek<T> operator+(const Zlomek<T>& x, const Zlomek<T>& y) {
	return Zlomek<T>(x.valUp * y.valDown + x.valDown * y.valUp, x.valDown * y.valDown);
}

template<typename T> string Zlomek<T>::result() {
	return to_string(valUp) + "/" + to_string(valDown);
}