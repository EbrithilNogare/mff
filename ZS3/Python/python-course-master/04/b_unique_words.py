#!/usr/bin/env python3

import sys

fh = open(sys.argv[1])
words = set()
for line in fh:
    for word in line.split():
        words.add(word)
fh.close()

print(len(words))
