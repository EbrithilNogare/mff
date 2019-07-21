#!/usr/bin/env python3

import sys

fh = open(sys.argv[1])
for line in fh:
    for word in line.split():
        print(word)
fh.close()
