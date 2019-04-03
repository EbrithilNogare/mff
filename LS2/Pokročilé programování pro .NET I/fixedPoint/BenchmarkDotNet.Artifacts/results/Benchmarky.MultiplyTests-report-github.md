``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.379 (1809/October2018Update/Redstone5)
Intel Core M-5Y10c CPU 0.80GHz, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0


```
|     Method |        Mean |     Error |    StdDev |      Median |
|----------- |------------:|----------:|----------:|------------:|
|  Q24_8Test | 440.1905 ns | 6.1784 ns | 5.4770 ns | 437.2297 ns |
| Q16_16Test | 444.9069 ns | 1.9564 ns | 1.7343 ns | 445.4677 ns |
|  Q8_24Test | 436.3857 ns | 0.8751 ns | 0.8185 ns | 436.7971 ns |
|  FloatTest |   0.0401 ns | 0.0126 ns | 0.0118 ns |   0.0467 ns |
| DoubleTest |   0.0515 ns | 0.0293 ns | 0.0274 ns |   0.0375 ns |
