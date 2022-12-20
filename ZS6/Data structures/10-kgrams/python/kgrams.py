class SuffixArray:
    def __init__(self, text):
        self.text = text
        # S is the suffix array (a permutation which sorts suffixes)
        # R is the ranking array (the inverse of S)
        self.R, self.S = self._build_suffix_array(text)

    def _build_suffix_array(self, text):
        """
        Construct the suffix array and ranking array for the given string
        using the doubling algorithm.
        """

        n = len(text)
        R = [None] * (n+1)
        S = [None] * (n+1)

        R = self._sort_and_rank(S, lambda a: ord(text[a]) if a < len(text) else -1)

        k = 1
        while k < n:
            R = self._sort_and_rank(S, lambda a: (R[a], (R[a+k] if a+k < n else -1)))
            k *= 2

        return (tuple(R), tuple(S))

    # An auxiliary function used in the doubling algorithm.
    def _sort_and_rank(self, S, key):
        for i in range(len(S)): S[i] = i
        S.sort(key = key)

        R = [None] * len(S)
        for i, s in enumerate(S):
            prev_s = S[i-1]
            if i == 0 or key(prev_s) != key(s): R[s] = i
            else: R[s] = R[prev_s]
        return R

    def num_kgrams(self, k):
        """Return the number of distinct k-grams in the string."""

        rrr = [x for x in self.R]
        sss = [x for x in self.S]
        print("k:")
        print(k)
        print("text:")
        print(self.text)
        print("R:")
        print(rrr)
        print("S:")
        print(sss)
        for i in range(0, len(sss)):
            r = rrr[i]
            s = sss[i]
            print("i: " + str(i))
            print("r: " + str(r))
            print("s: " + str(s))
            print(self.text[r:])
            print(self.text[s:])

        
        last_seen = (0,0)
        count = 0
        lenght = len(self.text)
        for s in self.S:
            if s+k <= lenght and self.text[s:s+k] != self.text[last_seen[0]:last_seen[1]]:
                last_seen = (s, s+k)
                count += 1
        return count
