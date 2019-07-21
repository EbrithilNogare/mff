#!/usr/bin/env python3
"""
Path elements.
"""

import sys


def usage():
    """
    Usage.
    """
    print(f'{sys.argv[0]} "<path>"')


if __name__ == "__main__":
    if len(sys.argv) != 2:
        usage()
        exit()

    PATH = (sys.argv[1].split('/'))
    EXT = PATH[-1].split('.')
    print("Path elements:")
    for i in PATH:
        print(i, end=' ')

    print()

    if len(EXT) > 1:
        print(f"Extension: {EXT[-1]}")
