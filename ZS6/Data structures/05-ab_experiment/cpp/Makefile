STUDENT_ID ?= PLEASE_SET_STUDENT_ID

.PHONY: test
test: ab_experiment
	@rm -rf out && mkdir out
	@for test in insert min random ; do \
		for mode in '2-3' '2-4' ; do \
			echo t-$$test-$$mode ; \
			./ab_experiment $$test $(STUDENT_ID) $$mode >out/t-$$test-$$mode ; \
		done ; \
	done

INCLUDE ?= .
CXXFLAGS=-std=c++11 -O2 -Wall -Wextra -g -Wno-sign-compare -I$(INCLUDE)

ab_experiment: ab_tree.h ab_experiment.cpp $(INCLUDE)/random.h
	$(CXX) $(CPPFLAGS) $(CXXFLAGS) $^ -o $@

.PHONY: clean
clean::
	rm -f ab_experiment
	rm -rf out
