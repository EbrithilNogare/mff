``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.379 (1809/October2018Update/Redstone5)
Intel Core M-5Y10c CPU 0.80GHz, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0


```
|     Method |        Mean |     Error |    StdDev |
|----------- |------------:|----------:|----------:|
|  Q24_8Test | 448.4870 ns | 2.2938 ns | 2.1457 ns |
| Q16_16Test | 460.0918 ns | 2.2544 ns | 2.1087 ns |
|  Q8_24Test | 446.8458 ns | 0.2837 ns | 0.2369 ns |
|  FloatTest |   0.0306 ns | 0.0118 ns | 0.0110 ns |
| DoubleTest |   0.0263 ns | 0.0046 ns | 0.0043 ns |
