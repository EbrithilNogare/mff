#include <functional>
#include <cstdlib>
#include <vector>

#include "cuckoo_hash.h"

void simple_test(unsigned n, unsigned table_size_percentage)
{
    CuckooTable table(n * table_size_percentage / 100);

    for (unsigned i=0; i < n; i++)
        table.insert(37*i);

    for (unsigned i=0; i < n; i++) {
        EXPECT(table.lookup(37*i), "Item not present in table, but it should be.");
        EXPECT(!table.lookup(37*i+1), "Item present in table, even though it should not be.");
    }
}

void multiple_test(unsigned min_n, unsigned max_n, unsigned step_n, unsigned table_size_percentage)
{
    for (unsigned n=min_n; n < max_n; n += step_n) {
        printf("\tn=%u\n", n);
        simple_test(n, table_size_percentage);
    }
}

/*** A list of all tests ***/

vector<pair<string, function<void()>>> tests = {
    { "small",     [] { simple_test(100, 400); } },
    { "middle",    [] { simple_test(31415, 300); } },
    { "big",       [] { simple_test(1000000, 300); } },
    { "tight",     [] { multiple_test(20000, 40000, 500, 205); } },
};
