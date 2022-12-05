class Matrix:
    """Interface of a matrix.

    This class provides only the matrix size N and a method for swapping
    two items. The actual storage of the matrix in memory is provided by
    subclasses in testing code.
    """

    def __init__(self, N):
        self.N = N

    def swap(self, i1, j1, i2, j2):
        """Swap elements (i1,j1) and (i2,j2)."""

        # Overridden in subclasses
        raise NotImplementedError

    def transpose(self):
        """Transpose the matrix."""
        self.transpose_recurse(0,0,self.N)


    def transpose_recurse(self, i, j, width):
        """ recursive transpose function """
        if width <= 1:
            return

        right_width = width // 2
        left_width = width - right_width

        self.transpose_recurse(i, j, left_width)
        self.transpose_block(i + left_width, j, right_width, left_width)
        self.transpose_recurse(i + left_width, j + left_width, right_width)

    def transpose_block(self, i, j, height, width):
        """ transpose on block """
        if width == 1 and height == 1:
            self.swap(i, j, j, i)
            return

        if height <= width:
            right_width = width // 2
            left_width = width - right_width

            self.transpose_block(j, i, left_width, height)
            self.transpose_block(j + left_width, i, right_width, height)
        else:
            right_height = height // 2
            left_height = height - right_height

            self.transpose_block(i, j, left_height, width)
            self.transpose_block(i + left_height, j, right_height, width)
