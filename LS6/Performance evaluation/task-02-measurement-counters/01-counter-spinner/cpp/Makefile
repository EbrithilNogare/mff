
all: counter-spinner

.PHONY: all run clean

counter-spinner: counter-spinner.cpp harness.cpp harness.h
	$(CXX) $(CXXFLAGS) -O3 -Wall -o $@ counter-spinner.cpp harness.cpp -lpapi

run: counter-spinner
	./counter-spinner >out.csv

clean:
	rm -f counter-spinner
