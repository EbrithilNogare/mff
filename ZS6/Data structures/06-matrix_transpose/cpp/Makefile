test: matrix_transpose_test
	./$<

CXXFLAGS=-std=c++11 -O2 -Wall -Wextra -g -Wno-sign-compare

matrix_transpose_test: matrix_transpose_test.cpp matrix_transpose.h matrix_tests.h test_main.cpp
	$(CXX) $(CXXFLAGS) $(filter-out %.h,$^) -o $@

clean:
	rm -f matrix_transpose_test

.PHONY: clean test
