test: kgrams_test
	./$<

INCLUDE ?= .
CXXFLAGS=-std=c++11 -O2 -Wall -Wextra -g -Wno-sign-compare -I$(INCLUDE)

kgrams_test: kgrams_test.cpp kgrams.h test_main.cpp
	$(CXX) $(CXXFLAGS) $(filter %.cpp,$^) -o $@

clean:
	rm -f kgrams_test

.PHONY: clean test
