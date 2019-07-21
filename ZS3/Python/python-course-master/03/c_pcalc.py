#!/usr/bin/env python3
"""
Polish calc.
"""
import sys


def usage():
    """
    Usage.
    """
    print(f"Usage: {sys.argv[0]} <POLISH NOTATION EXPRESSION>")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        usage()
        exit()

    STACK = []
    for i in sys.argv[1:]:
        if i in "+-*/%":
            b = STACK.pop()
            a = STACK.pop()
            c = eval(a + i + b)  # Danger!
            STACK.append(str(c))
        else:
            STACK.append(i)

    # Correct computation should leave one just one element
    if len(STACK) != 1:
        print("Err")
        exit(0)

    print(STACK[0])
