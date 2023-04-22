# Analyze Memory Accesses (with Pin)
- task version: Advanced Level


## PC spec:
- PC in LAB: u-pl4
- OS: Linux
- CPU: Intel(R) Core(TM) i7-6700 CPU @ 3.40GHz
- gcc version: 12.2.1 20230304
- PIN version: pin-3.27-98718-gbeaa5d51e-gcc-linux


## How it works
Program is handling instructions of memory allocation and read / write from memory.

Those instructions are handled and for each memory block (int, byte, class ...) is created record in map.

Each operation can create these record, alternate them and in the end go through the records and sum needed data.

On output are count (in bytes) of memory with some behaviour.


### program output:
```
the total amount of memory that was allocated: 324212b
the amount of memory that was allocated but never accessed (neither read nor written): 0b
the amount of memory that was written but never read: 67540b
the amount of memory that was read before being written: 0b
the amount of memory that was not read after last write: 219524b
```
