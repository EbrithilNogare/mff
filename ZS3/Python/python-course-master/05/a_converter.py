#!/usr/bin/env python3

import sys


usage = lambda: print("Usage: sys.argv[0] [-U | -L] file")

len(sys.argv) == 2 or usage()
len(sys.argv) == 2 or sys.exit(1)

sys.argv[1] == '-U' and print(sys.stdin.read().upper())
sys.argv[1] == '-L' and print(sys.stdin.read().lower())
