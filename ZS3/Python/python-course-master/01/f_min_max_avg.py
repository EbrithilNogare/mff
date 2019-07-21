#!/usr/bin/env python3
"""
Min, Max, Avg.
"""

import sys


def avg(num_list):
    """
    Avg.
    """
    if not num_list:
        return 0

    return sum(num_list) / len(num_list)


if __name__ == "__main__":
    INPUT = []
    for i in sys.argv[1:]:
        if int(i) >= 0:
            INPUT.append(int(i))

    print(min(INPUT), max(INPUT), avg(INPUT))
