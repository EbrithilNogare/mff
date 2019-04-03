``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.379 (1809/October2018Update/Redstone5)
Intel Core M-5Y10c CPU 0.80GHz, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0


```
|     Method |        Mean |     Error |    StdDev |
|----------- |------------:|----------:|----------:|
|  Q24_8Test | 435.5379 ns | 2.4758 ns | 2.1948 ns |
| Q16_16Test | 436.3750 ns | 3.0095 ns | 2.5131 ns |
|  Q8_24Test | 435.7604 ns | 1.0065 ns | 0.7858 ns |
|  FloatTest |   0.0384 ns | 0.0148 ns | 0.0132 ns |
| DoubleTest |   0.0443 ns | 0.0162 ns | 0.0143 ns |
