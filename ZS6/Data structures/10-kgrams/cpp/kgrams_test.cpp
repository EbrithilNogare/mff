#include <algorithm>
#include <functional>
#include <string>
#include <vector>
#include <cstdint>

using namespace std;

// If the condition is not true, report an error and halt.
#define EXPECT(condition, message) do { if (!(condition)) expect_failed(message); } while (0)
void expect_failed(const string& message);

#include "kgrams.h"

void test_generic(const string& text, int k, int expected_kg)
{
    SuffixArray sa(text);
    int num_kg = sa.num_kgrams(k);
    EXPECT(num_kg == expected_kg, "Expected " + to_string(expected_kg) + " " + to_string(k) + "-grams, found " + to_string(num_kg) + ".");

}

// Test on a fixed string.
void test_explicit(int k, int expected_kg)
{
    test_generic("annbansbananas", k, expected_kg);
}

// Test on a very non-uniform random string.
void test_random(int n, int k, int expected_kg)
{
    string s(n, ' ');
    uint32_t state = n;

    for (int i=0; i<n; i++) {
        state = state*2654289733 + 7;
        unsigned x = (state >> 28) % 16;
        char next = "aaaaaaaaaaaabbbc"[x];
        s[i] = next;
    }

    test_generic(s, k, expected_kg);
}

// Test on an almost-constant string.
void test_trivial(int n, int k, int expected_kg)
{
    string s(n, 'a');

    for (int i=0; i<n; i++) {
        if (i == n/2)
            s[i] = 'b';
        else
            s[i] = 'a';
    }

    test_generic(s, k, expected_kg);
}

vector<pair<string, function<void()>>> tests = {
    {"basic-1",     [] { test_explicit(1, 4); }},
    {"basic-2",     [] { test_explicit(2, 8); }},
    {"basic-3",     [] { test_explicit(3, 10); }},
    {"basic-4",     [] { test_explicit(4, 11); }},
    {"basic-14",    [] { test_explicit(14, 1); }},

    {"short-5",     [] { test_random(1000, 5, 107); }},
    {"short-33",    [] { test_random(1000, 33, 968); }},
    {"short-500",   [] { test_random(1000, 500, 501); }},

    {"long-5",      [] { test_random(100000, 5, 230); }},
    {"long-33",     [] { test_random(100000, 33, 99767); }},
    {"long-5000",   [] { test_random(100000, 5000, 95001); }},

    {"triv-1",      [] { test_trivial(1000000, 1, 2); }},
    {"triv-5",      [] { test_trivial(1000000, 5, 6); }},
    {"triv-3333",   [] { test_trivial(1000000, 3333, 3334); }},
    {"triv-500000", [] { test_trivial(1000000, 500000, 500001); }},
};
