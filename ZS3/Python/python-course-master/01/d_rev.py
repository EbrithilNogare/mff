#!/usr/bin/env python3
"""
Printing in reverse order.
"""

import sys

if __name__ == "__main__":
    for i in range(len(sys.argv) - 1, 0, -1):
        print(sys.argv[i])
