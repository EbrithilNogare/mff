# Prove of false sharing with PAPI library

## PC spec:
- PC in LAB: u-pl4
- OS: Linux
- CPU: Intel(R) Core(TM) i7-6700 CPU @ 3.40GHz
- CPU cores / threads: 4 / 8
- CPU L1 size: 128 KiB 
- gcc version: 12.2.1 20230304

## Measuing 
Measuring instruction `PAPI_L1_DCM` on 100,000 cycles.

#### Compilation:
must be added `-lpapi` param to have references
```
cc  -O3 -Wall -pthread -o false-sharing false-sharing.c -lpapi
```

## Measured data

### Distance 0

``` bash
/false-sharing 0 1 2 3 4 5 6 7
```

|  CPU  | L1 misses |  CPU  | L1 misses |
|-------|----------:|-------|----------:|
| CPU 0 |     49607 | CPU 1 |     49600 |
| CPU 0 |     56577 | CPU 2 |     56540 |
| CPU 0 |     54957 | CPU 3 |     54910 |
| CPU 0 |        45 | CPU 4 |        45 |
| CPU 0 |     49999 | CPU 5 |     50016 |
| CPU 0 |     56435 | CPU 6 |     56410 |
| CPU 0 |     52391 | CPU 7 |     52399 |

### Distance 64

``` bash
/false-sharing 64 1 2 3 4 5 6 7
```

|  CPU  | L1 misses |  CPU  | L1 misses |
|-------|----------:|-------|----------:|
| CPU 0 |        50 | CPU 1 |        20 |
| CPU 0 |        36 | CPU 2 |        55 |
| CPU 0 |        54 | CPU 3 |        41 |
| CPU 0 |        29 | CPU 4 |        29 |
| CPU 0 |        46 | CPU 5 |        41 |
| CPU 0 |        44 | CPU 6 |        45 |
| CPU 0 |        40 | CPU 7 |        35 |

## Result
From tables above we can see as we reduce distance to zero there is increase of L1 cache misses on all cores except 4th,

That means false sharing is happening.

Another observation Core 0 has same L1 cache as Core 4.
It means Core 4 is hyperthread of Core 0. 
