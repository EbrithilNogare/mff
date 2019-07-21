#!/usr/bin/env python3

import sys

limit = 10
fh = open(sys.argv[1])
printed = ""
for line in fh:
    for word in line.split():
        for char in word:
            printed = printed + char
            if len(printed) == limit:
                print(printed)
                printed = ""
        printed = printed + " "
fh.close()
