#!/usr/bin/env python3
"""
n+nn+nnn
"""

import sys


def usage():
    """
    Usage.
    """
    print(f'{sys.argv[0]} "<n>"')


if __name__ == "__main__":
    if len(sys.argv) != 2:
        usage()
        exit()

    N = int(sys.argv[1])
    SUM = N + int(2*str(N)) + int(3*str(N))
    print(SUM)
