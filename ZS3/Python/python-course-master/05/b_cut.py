#!/usr/bin/env python3

import sys


usage = lambda: print("Usage: sys.argv[0] -d delim -f field")

len(sys.argv) == 5 or usage()
len(sys.argv) == 5 or exit(1)
delimiter = sys.argv[2]
fields = int(sys.argv[4])

parsed = list(map(lambda x: x.split(delimiter), sys.stdin.readlines()))
print("\n".join(list(map(lambda x: x[fields], parsed))))
