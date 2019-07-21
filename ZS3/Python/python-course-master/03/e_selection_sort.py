#!/usr/bin/env python3
"""
Selection sort.
"""


def selection_sort(array):
    """
    Selection sort function.
    """
    for i in range(len(array)):
        argmin = array.index(min(array[i:]))
        array[i], array[argmin] = array[argmin], array[i]


if __name__ == "__main__":
    ARRAY = [i for i in range(10, 0, -1)]
    print(ARRAY)
    selection_sort(ARRAY)
    print(ARRAY)
