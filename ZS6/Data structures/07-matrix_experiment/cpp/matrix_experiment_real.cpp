#include <functional>
#include <string>
#include <vector>
#include <cstdio>
#include <cmath>
#include <iostream>

#include <time.h>

using namespace std;

// If the condition is not true, report an error and halt.
#define EXPECT(condition, message) do { if (!(condition)) expect_failed(message); } while (0)

void expect_failed(const string& message) {
    cerr << "Test error: " << message << endl;
    exit(1);
}

class Matrix {
    vector<unsigned> items;
    unsigned &item(unsigned i, unsigned j) { return items[i*N + j]; }
  public:
    unsigned N;
    Matrix(unsigned N) { this->N = N; items.resize(N*N, 0); }

    void swap(unsigned i1, unsigned j1, unsigned i2, unsigned j2)
    {
        // EXPECT(i1 < N && j1 < N && i2 < N && j2 < N, "Swap out of range: " + coord_string(i1, j1) + " with " + coord_string(i2, j2) + ".");
        std::swap(item(i1, j1), item(i2, j2));
    }

    void naive_transpose()
    {
        for (unsigned i=0; i<N; i++)
            for (unsigned j=0; j<i; j++)
                swap(i, j, j, i);
    }

#include "matrix_transpose.h"
};

void real_test(bool naive)
{
    for (int e=40; e<=112; e++) {
        unsigned N = (unsigned) pow(2, e/8.);
        Matrix m(N);

        clock_t start_time, stop_time;
        unsigned tries = 1;
        do {
            start_time = clock();
            for (unsigned t=0; t < tries; t++) {
                if (naive)
                    m.naive_transpose();
                else
                    m.transpose();
            }
            stop_time = clock();
            tries *= 2;
        } while (stop_time - start_time < CLOCKS_PER_SEC/10);
        // It is guaranteed that the total number of tries is odd :)

        double ns_per_item = (double)(stop_time - start_time) / CLOCKS_PER_SEC / (N*(N-1)) / tries * 1e9;
        printf("%d\t%.6f\n", N, ns_per_item);
    }
}

int main(int argc, char **argv)
{
    if (argc != 2) {
        fprintf(stderr, "Usage: %s (smart|naive)\n", argv[0]);
        return 1;
    }

    std::string mode = argv[1];

    bool naive;
    if (mode == "smart")
      naive = false;
    else if (mode == "naive")
      naive = true;
    else {
        fprintf(stderr, "The argument must be either 'smart' or 'naive'\n");
        return 1;
    }

    real_test(naive);
    return 0;
}
