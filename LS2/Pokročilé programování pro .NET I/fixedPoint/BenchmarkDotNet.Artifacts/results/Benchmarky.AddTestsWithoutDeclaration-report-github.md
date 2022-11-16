``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.379 (1809/October2018Update/Redstone5)
Intel Core M-5Y10c CPU 0.80GHz, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0


```
|     Method |        Mean |     Error |    StdDev |
|----------- |------------:|----------:|----------:|
|  Q24_8Test | 168.3616 ns | 0.7389 ns | 0.6911 ns |
| Q16_16Test | 157.6584 ns | 0.8073 ns | 0.6741 ns |
|  Q8_24Test | 152.3980 ns | 0.6149 ns | 0.5451 ns |
|  FloatTest |   0.0000 ns | 0.0000 ns | 0.0000 ns |
| DoubleTest |   0.0570 ns | 0.0180 ns | 0.0150 ns |
