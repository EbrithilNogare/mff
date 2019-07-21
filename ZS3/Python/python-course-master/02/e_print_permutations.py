#!/usr/bin/env python3
"""
Print permutations
"""

import sys

PERMS = []


def perm_naive(head, tail):
    """
    Naive permutation computation.
    """
    if not tail and head not in PERMS:
        PERMS.append(head)
        return

    for i in range(len(tail)):
        perm_naive(head + tail[i], tail[:i] + tail[i+1:])


def permutation(string):
    """
    Wrapper for perm_naive for hiding global variabl.
    """
    perm_naive("", string)
    return PERMS


def usage():
    """
    Usage.
    """
    print(f'{sys.argv[0]} "<string>"')


if __name__ == "__main__":
    if len(sys.argv) != 2:
        usage()
        exit()

    STRING = sys.argv[1]
    print(permutation(STRING))
