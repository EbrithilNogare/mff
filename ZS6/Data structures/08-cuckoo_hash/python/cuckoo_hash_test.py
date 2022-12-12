#!/usr/bin/env python3
import sys
import random

from cuckoo_hash import CuckooTable

def simple_test(n, table_size_percentage):
    random.seed(42)
    table = CuckooTable(n*table_size_percentage//100)

    # Insert an arithmetic progression
    for i in range(n):
        table.insert(37*i)

    # Verify contents of the table
    for i in range(n):
        assert table.lookup(37*i), "Item not present in table, but it should be."
        assert not table.lookup(37*i+1), "Item present in table, even though it should not be."

def multiple_test(min_n, max_n, step_n, table_size_percentage):
    for n in range(min_n, max_n, step_n):
        print("\tn={}".format(n))
        simple_test(n, table_size_percentage)

# A list of all tests
tests = [
    ("small",       lambda: simple_test(100, 400)),
    ("middle",      lambda: simple_test(31415, 300)),
    ("big",         lambda: simple_test(1000000, 300)),
    ("tight",       lambda: multiple_test(20000, 40000, 500, 205)),
]

if __name__ == "__main__":
    for required_test in sys.argv[1:] or [name for name, _ in tests]:
        for name, test in tests:
            if name == required_test:
                print("Running test {}".format(name), file=sys.stderr)
                test()
                break
        else:
            raise ValueError("Unknown test {}".format(name))
