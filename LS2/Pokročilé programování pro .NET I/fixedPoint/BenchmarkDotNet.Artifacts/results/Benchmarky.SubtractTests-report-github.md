``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.379 (1809/October2018Update/Redstone5)
Intel Core M-5Y10c CPU 0.80GHz, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0


```
|     Method |        Mean |     Error |    StdDev |
|----------- |------------:|----------:|----------:|
|  Q24_8Test | 437.7443 ns | 1.5082 ns | 1.1775 ns |
| Q16_16Test | 447.3305 ns | 2.3503 ns | 1.8349 ns |
|  Q8_24Test | 437.4870 ns | 0.3309 ns | 0.2933 ns |
|  FloatTest |   0.0000 ns | 0.0000 ns | 0.0000 ns |
| DoubleTest |   0.0574 ns | 0.0057 ns | 0.0053 ns |
