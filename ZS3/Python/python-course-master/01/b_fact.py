#!/usr/bin/env python3
"""
Factorial.
"""

N = 5


def fact(i):
    """
    Factorial function.
    """
    if i < 2:
        return 1

    return i*fact(i - 1)


if __name__ == "__main__":
    print(fact(N))
