#!/usr/bin/env python3

class Node:
    """Node in a binary tree `Tree`"""

    def __init__(self, key, left=None, right=None, parent=None):
        self.key = key
        self.left = left
        self.right = right
        self.parent = parent

class Tree:
    """A simple binary search tree"""

    def __init__(self, root=None):
        self.root = root

    def insert(self, key):
        """Insert key into the tree.

        If the key is already present, do nothing.
        """
        if self.root is None:
            self.root = Node(key)
            return

        node = self.root
        while node.key != key:
            if key < node.key:
                if node.left is None:
                    node.left = Node(key, parent=node)
                node = node.left
            else:
                if node.right is None:
                    node.right = Node(key, parent=node)
                node = node.right

    def successor(self, node=None):
        """Return successor of the given node.

        The successor of a node is the node with the next greater key.
        Return None if there is no such node.
        If the argument is None, return the node with the smallest key.
        """
        newNode = node
        if newNode is None:
            newNode = self.root
            while newNode.left is not None:
                newNode = newNode.left
            return newNode
        elif newNode.right is not None:
            newNode = newNode.right
            while newNode.left is not None:
                newNode = newNode.left
            return newNode
        elif newNode.parent is not None:
            while newNode.parent is not None and newNode.parent.key < newNode.key:
                newNode = newNode.parent
            return newNode.parent
        else:
            return None
