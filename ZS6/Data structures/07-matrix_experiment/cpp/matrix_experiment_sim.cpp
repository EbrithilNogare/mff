#include <functional>
#include <string>
#include <vector>
#include <cstdio>
#include <cmath>
#include <string>
#include <iostream>

#include <time.h>

using namespace std;

// If the condition is not true, report an error and halt.
#define EXPECT(condition, message) do { if (!(condition)) expect_failed(message); } while (0)

void expect_failed(const string& message) {
    cerr << "Test error: " << message << endl;
    exit(1);
}

#include "matrix_tests.h"

void simulated_test(unsigned M, unsigned B, bool naive)
{
    for (int e=20; e<=52; e++) {
        unsigned N = (unsigned) pow(2, e/4.);
        TestMatrix m(N, M, B, 0);
        m.fill_matrix();
        m.reset_stats();
        if (naive)
            m.naive_transpose();
        else
            m.transpose();

        double misses_per_item = (double) m.stat_cache_misses / (N*(N-1));
        printf("%d\t%.6f\n", N, misses_per_item);

        m.check_result();
    }
}

vector<pair<string, function<void(bool n)>>> tests = {
//                                                    M     B
    { "m1024-b16",    [](bool n) { simulated_test( 1024,   16, n); } },
    { "m8192-b64",    [](bool n) { simulated_test( 8192,   64, n); } },
    { "m65536-b256",  [](bool n) { simulated_test(65536,  256, n); } },
    { "m65536-b4096", [](bool n) { simulated_test(65536, 4096, n); } },
};

int main(int argc, char **argv)
{
    if (argc != 3) {
        fprintf(stderr, "Usage: %s <test> (smart|naive)\n", argv[0]);
        return 1;
    }

    std::string which_test = argv[1];
    std::string mode = argv[2];

    bool naive;
    if (mode == "smart")
      naive = false;
    else if (mode == "naive")
      naive = true;
    else
      {
        fprintf(stderr, "Last argument must be either 'smart' or 'naive'\n");
        return 1;
      }

    for (const auto& test : tests) {
        if (test.first == which_test) {
            test.second(naive);
            return 0;
        }
    }

    fprintf(stderr, "Unknown test %s\n", which_test.c_str());
    return 1;
}
