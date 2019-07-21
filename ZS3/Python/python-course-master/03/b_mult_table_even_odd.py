#!/usr/bin/env python3
"""
Multiplication table.
"""
import sys


def usage():
    """
    Usage.
    """
    print(f"Usage: {sys.argv[0]} <number>")


if __name__ == "__main__":
    if len(sys.argv) != 2:
        usage()
        exit()

    NUM = int(sys.argv[1])
    MTABLE = [(i, j, i*j) for i in range(1, 11) for j in range(1, 11)]
    ODD = [x for x in MTABLE if x[2] % 2 != 0]
    EVEN = [x for x in MTABLE if x[2] % 2 == 0]
    print(ODD)
    print(EVEN)
