#!/usr/bin/env python3
"""
Count primes between two integers.
"""

import sys


def usage():
    """
    Usage.
    """
    print(f'{sys.argv[0]} "<n> <n>"')


def is_prime(number):
    """
    True if :number: is prime.
    """
    if number <= 3:
        return True

    for j in range(4, int(number / 2) + 1):
        if number % j == 0:
            return False

    return True


if __name__ == "__main__":
    if len(sys.argv) != 3:
        usage()
        exit()

    FROM = int(sys.argv[1])
    TO = int(sys.argv[2])

    if FROM > TO:
        FROM, TO = TO, FROM

    PRIMES = 0
    for i in range(FROM, TO + 1):
        if is_prime(i):
            PRIMES += 1

    print(PRIMES)
