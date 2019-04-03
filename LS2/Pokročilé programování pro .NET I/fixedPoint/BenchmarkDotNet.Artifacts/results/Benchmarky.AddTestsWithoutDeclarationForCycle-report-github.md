``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.379 (1809/October2018Update/Redstone5)
Intel Core M-5Y10c CPU 0.80GHz, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0


```
|     Method |         Mean |      Error |     StdDev |
|----------- |-------------:|-----------:|-----------:|
|  Q24_8Test | 1,622.679 us | 15.1992 us | 12.6920 us |
| Q16_16Test | 1,563.110 us | 30.0806 us | 28.1374 us |
|  Q8_24Test | 1,509.777 us |  7.6786 us |  6.4120 us |
|  FloatTest |     6.331 us |  0.0249 us |  0.0221 us |
| DoubleTest |    10.083 us |  0.0508 us |  0.0397 us |
