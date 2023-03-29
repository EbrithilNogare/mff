# Spin Hardware Counters

## PC spec:
- PC in LAB: u-pl4
- OS: Linux
- CPU: Intel(R) Core(TM) i7-6700 CPU @ 3.40GHz
- CPU cores / threads: 4 / 8
- CPU L3 size: 8MBi 
- gcc version: 12.2.1 20230304

## Fast results
Number of instruction to rise counter by one (lower is better)
| FP_ARITH:SCALAR_DOUBLE | PAPI_BR_MSP | PAPI_L3_TCM |
|:----------------------:|:-----------:|:-----------:|
|                  8.547 |      142.85 |        0.69 |


## Measured data

### FP_ARITH:SCALAR_DOUBLE
FP_ARITH:SCALAR_DOUBLE refers to double-precision (64-bit) floating-point arithmetic operations on individual values.
``` cpp
volatile double arrayA [32] = {.1};
volatile double arrayB [32] = {.2};
volatile double arrayC [32] = {.3};

int execute() {
    for (size_t i = 0; i < 32; ++i)
        arrayA[i] = arrayB[i] * arrayB[i];
    return arrayA[0];
}
```
There should be theoretically counter hit every 7sth operation and real data shows **8.5** (instructions for event), which is pretty close.

| operation                 | instruction count | note             |
|:--                        |--:                |------------------|
| comparsion                | 32 x 2            | mask and compare |
| load from memory          | 32 x 2            |                  |
| store to memory           | 32                |                  |
| raise counter             | 32                |                  |
| **double multiplication** | 32                |                  |



### PAPI_BR_MSP
PAPI_BR_MSP is a counter measuring the number of mispredicted branch events.

I may seem as primitive solution, but after 2 days of implementing different hash functions I wasnt able to beat ratio of instruction / event made by random generator.

``` cpp
int iterator = 0;
int sum = 0;

void before() override {
    std::srand(std::time(nullptr)); // init random generator
}

int execute() {
    if (std::rand() % 2 == 0)
        sum += iterator++;
    else
        sum -= iterator++;
    return sum;
}
```

I assume hardware RND is on the machine so number of instructions is relative small for `std::rand()` method.

I got to the score of one miss-predicted branch every **140** instructions.


### PAPI_L3_TCM
PAPI_L3_TCM is a performance counter, which measures the number of cache misses that occur in the last-level cache (LLC) of a processor.

``` cpp
int iterator = 0;
// for machine with 8MBi of L3 and loading 2 pages at a time
int megaArray[3*8*1024*1024];

void before() override s{
    // this could be lower number (256) but why not
    const int pageSize = 1024;
    int length = std::end(megaArray) - std::begin(megaArray);
    for(int i = 0; i<length;i++)
        // fill array with carefuly taken poiters / iterators of this array
        megaArray[i] = (i + pageSize) % length;
}

int execute() {
    // unrolled For cycle 16x
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    iterator = megaArray[iterator];
    return iterator;
}
```

I got 1 miss cache for every **0.69** instructions (Yep I double checked that number).

Here happened some dark magic and cache miss occured more often than
calling instructions.

I am guessing (I didnt found an explanation)
- That may be because of loading same wrong cache multiple times
- Optimalization of multiple same instructions into one instruction
- Loading two caches at a time may count as double cache misse (my favourite)


