#define DS1_RANDOM_H

#include <cstdint>

/*
 * This is the xoroshiro128+ random generator, designed in 2016 by David Blackman
 * and Sebastiano Vigna, distributed under the CC-0 license. For more details,
 * see http://vigna.di.unimi.it/xorshift/.
 *
 * Rewritten to C++ by Martin Mares, also placed under CC-0.
 */

class RandomGen {
    uint64_t state[2];

    uint64_t rotl(uint64_t x, int k)
    {
        return (x << k) | (x >> (64 - k));
    }

  public:
    // Initialize the generator, set its seed and warm it up.
    RandomGen(unsigned int seed)
    {
        state[0] = seed * 0xdeadbeef;
        state[1] = seed ^ 0xc0de1234;
        for (int i=0; i<100; i++)
            next_u64();
    }

    // Generate a random 64-bit number.
    uint64_t next_u64(void)
    {
        uint64_t s0 = state[0], s1 = state[1];
        uint64_t result = s0 + s1;
        s1 ^= s0;
        state[0] = rotl(s0, 55) ^ s1 ^ (s1 << 14);
        state[1] = rotl(s1, 36);
        return result;
    }

    // Generate a random 32-bit number.
    uint32_t next_u32(void)
    {
      return next_u64() >> 11;
    }

    // Generate a number between 0 and range-1.
    unsigned int next_range(unsigned int range)
    {
        /*
         * This is not perfectly uniform, unless the range is a power of two.
         * However, for 64-bit random values and 32-bit ranges, the bias is
         * insignificant.
         */
        return next_u64() % range;
    }
};

