#!/usr/bin/env python3
"""
Pine.
"""

import sys
import math


def gen_odds(maximum):
    """
    Generate odd numbers.
    """
    odd_numbers = []
    for i in range(1, maximum + 1, 2):
        odd_numbers.append(i)

    return odd_numbers


def print_pine(width):
    """
    Print pine.
    """
    odds = gen_odds(width)
    for i in odds:
        spaces_before = int((width - i) / 2)
        print(' ' * spaces_before, end='')
        print('*' * i)

    # Print trunk
    for i in range(int(math.log(width))):
        print(' ' * int((width - 1) / 2), end='')
        print('*')


def usage():
    """
    Usage.
    """
    print(f"Usage: {sys.argv[0]} <height>")


if __name__ == "__main__":
    if len(sys.argv) != 2:
        usage()
        exit()
    HEIGHT = int(sys.argv[1])
    print_pine(HEIGHT)
