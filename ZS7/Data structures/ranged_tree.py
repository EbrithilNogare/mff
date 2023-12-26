class RangeNode:
    """Node in a range tree"""

    def __init__(self, start, end, left=None, right=None, parent=None):
        self.start = start
        self.end = end
        self.parent = parent
        self.left = left
        self.right = right
        if left is not None:
            left.parent = self
        if right is not None:
            right.parent = self

class RangeTree:
    """A simple range tree"""

    def __init__(self, root=None):
        self.root = root
    
    def rotate(self, node):
        """ Rotate the given `node` up.

        Performs a single rotation of the edge between the given node
        and its parent, choosing left or right rotation appropriately.
        """
        if node.parent is not None:
            if node.parent.left == node:
                if node.right is not None: node.right.parent = node.parent
                node.parent.left = node.right
                node.right = node.parent
            else:
                if node.left is not None: node.left.parent = node.parent
                node.parent.right = node.left
                node.left = node.parent
            if node.parent.parent is not None:
                if node.parent.parent.left == node.parent:
                    node.parent.parent.left = node
                else:
                    node.parent.parent.right = node
            else:
                self.root = node
            node.parent.parent, node.parent = node, node.parent.parent

    def search_range(self, start, end):
        """Search for intervals that overlap with the given range."""
        result = []
        self._search_range(self.root, start, end, result)
        return result

    def _search_range(self, node, start, end, result):
        """Helper function for range search."""
        if node is None:
            return

        # Check if the node's range overlaps with the search range
        if start <= node.end and end >= node.start:
            result.append((node.start, node.end))

        # Recur on left and right subtrees based on the search range
        if node.left is not None and start <= node.left.end:
            self._search_range(node.left, start, end, result)
        if node.right is not None and end >= node.right.start:
            self._search_range(node.right, start, end, result)

    def insert_interval(self, start, end):
        """Insert a new interval into the range tree."""
        if self.root is None:
            self.root = RangeNode(start, end)
            return

        node = self.root
        while node is not None:
            # Check for overlap with the current node's range
            if start <= node.end and end >= node.start:
                # Update the node's range to cover the new interval
                node.start = min(start, node.start)
                node.end = max(end, node.end)
                return
            elif start < node.start:
                if node.left is None:
                    node.left = RangeNode(start, end, parent=node)
                    self.rotate(node.left)
                    return
                node = node.left
            else:
                if node.right is None:
                    node.right = RangeNode(start, end, parent=node)
                    self.rotate(node.right)
                    return
                node = node.right
