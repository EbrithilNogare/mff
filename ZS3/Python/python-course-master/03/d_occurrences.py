#!/usr/bin/env python3
"""
Occurrences.
"""
import sys


def usage():
    """
    Usage.
    """
    print(f"Usage: {sys.argv[0]} <STRING>")


if __name__ == "__main__":
    if len(sys.argv) != 2:
        usage()
        exit()

    WORD = sys.argv[1]
    for x in {c for c in WORD}:
        print(WORD.count(x))
