#!/usr/bin/env python3
import sys

def find_duplicates(data_generator):
    """Find duplicates in the given data.

    The `data_generator` is an iterable over strings, so it can be
    iterated for example using a `for` cycle:

      for item in data_generator: ...

    It can be iterated multiple times.

    The goal is to return a list of duplicated entries, reporting each duplicated
    entry only once.
    """
    t = Trie()
    foundWords = []
    MAX_DEPTH = 5

    #for index in data_generator:
    #    MAX_DEPTH = len(index) + 1
    #    break
    
    for item in data_generator:
        t.insert(bytearray(item[0:1], "utf8"))

    for index in range(2, MAX_DEPTH + 1):
        i = 0
        for itemZ in data_generator:
            item = bytearray(itemZ, "utf8")
            if i % 10000 == 0:
                print(i)
            i+=1

            item_prev = item[0:index-1]
            item_this = item[0:index]

            query = t.query(item_prev)
            if len(query) > 0 and query[0][1] > 1:
                t.insert(item_this)
            t.insert(item_prev)

    for itemZ in data_generator:
        item = bytearray(itemZ, "utf8")
        query = t.query(item[0:MAX_DEPTH])
        if len(query) > 0:
            t.insert(item)

    for itemZ in data_generator:
        item = bytearray(itemZ, "utf8")
        query = t.query(item)
        if len(query) > 0 and query[0][1] > 1 and item not in foundWords:
            foundWords.append(item)




    return foundWords

### Code below from https://albertauyeung.github.io/2020/06/15/python-trie.html/
class TrieNode:
    def __init__(self, char):
        self.char = char
        self.is_end = False
        self.counter = 0
        self.children = {}

class Trie(object):
    def __init__(self):
        self.root = TrieNode("")
    
    def insert(self, word):
        node = self.root
        for char in word:
            if char in node.children:
                node = node.children[char]
            else:
                new_node = TrieNode(char)
                node.children[char] = new_node
                node = new_node
        node.is_end = True
        node.counter += 1

    def dfs(self, node, prefix):
        if node.is_end:
            self.output.append((prefix + node.char, node.counter))
        for child in node.children.values():
            self.dfs(child, prefix + node.char)
        
    def query(self, x):
        self.output = []
        node = self.root
        for char in x:
            if char in node.children:
                node = node.children[char]
            else:
                return []
        self.dfs(node, x[:-1])
        return sorted(self.output, key=lambda x: x[1], reverse=True)
