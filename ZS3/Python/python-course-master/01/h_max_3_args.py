#!/usr/bin/env python3
"""
Maximum of three numbers given as its arguments.
"""

import sys


def max_3_numbers(num1, num2, num3):
    """
    Returns maximum of 3 numbers.
    """
    tmp = num1
    if num2 > num1:
        tmp = num2

    if num3 > tmp:
        return num3

    return tmp


def usage():
    """
    Usage.
    """
    print(f'{sys.argv[0]} "1st number" "2nd number" "3rd number"')


if __name__ == "__main__":
    if len(sys.argv) != 4:
        usage()
        exit()

    print(max_3_numbers(int(sys.argv[1]), int(sys.argv[2]), int(sys.argv[3])))
