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

    def transpose(self, i=0, j=0, width=None):
        """Transpose the matrix."""
        if width is None:
            width = self.N
        
        if width == 1 or width == 0:
            return

        first_half = width // 2
        second_half = width - first_half

        self.transpose(i, j, second_half)
        self.transpose_bounded(i + second_half, j, first_half, second_half)
        self.transpose(i + second_half, j + second_half, first_half)

    def transpose_bounded(self, i1, j1, i2, j2):
        """ transpose on block """
        if j2 * i2 == 1:
            self.swap(i1, j1, j1, i1)
            return

        if i2 <= j2:
            i2_half = j2 // 2
            i2_rest = j2 - i2_half

            self.transpose_bounded(j1, i1, i2_half, i2)
            self.transpose_bounded(j1 + i2_half, i1, i2_rest, i2)
        else:
            i2_half = i2 // 2
            i2_rest = i2 - i2_half

            self.transpose_bounded(i1, j1, i2_half, j2)
            self.transpose_bounded(i1 + i2_half, j1, i2_rest, j2)
