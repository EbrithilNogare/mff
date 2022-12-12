test: cuckoo_hash_test
	./$<

INCLUDE ?= .
CXXFLAGS=-std=c++11 -O2 -Wall -Wextra -g -Wno-sign-compare -I$(INCLUDE)

cuckoo_hash_test: cuckoo_hash_test.cpp cuckoo_hash.h test_main.cpp $(INCLUDE)/random.h
	$(CXX) $(CXXFLAGS) $^ -o $@

clean:
	rm -f cuckoo_hash_test

.PHONY: clean test
