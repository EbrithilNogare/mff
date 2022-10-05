#!/usr/bin/env python3
import sys

import tree_successor

def test_sequence(sequence):
    tree = tree_successor.Tree()
    for i in sequence: tree.insert(i)

    node = tree.successor(None)
    for element in sorted(sequence):
        assert node is not None, "Expected successor {}, got None".format(element)
        assert node.key == element, "Expected successor {}, got {}".format(element, node.key)
        node = tree.successor(node)
    assert node is None, "Expected no successor, got {}".format(node.key)

def gen_increasing_seq():
    return [int(7.13*i) for i in range(10000)]

tests = [
    ("right_path", lambda: test_sequence(gen_increasing_seq())),
    ("left_path", lambda: test_sequence(list(reversed(gen_increasing_seq())))),
    ("random_tree", lambda: test_sequence([pow(997, i, 199999) for i in range(1, 199999)])),
]

if __name__ == "__main__":
    for required_test in sys.argv[1:] or [name for name, _ in tests]:
        for name, test in tests:
            if name == required_test:
                print("Running test {}".format(name), file=sys.stderr)
                test()
                break
        else:
            raise ValueError("Unknown test {}".format(name))
