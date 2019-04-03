``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.379 (1809/October2018Update/Redstone5)
Intel Core M-5Y10c CPU 0.80GHz, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0


```
|     Method |      Mean |     Error |    StdDev |
|----------- |----------:|----------:|----------:|
|  Q24_8Test | 415.35 us | 2.1807 us | 2.0398 us |
| Q16_16Test | 414.50 us | 1.4994 us | 1.3292 us |
|  Q8_24Test | 414.38 us | 0.2628 us | 0.2458 us |
|  FloatTest | 160.36 us | 0.0933 us | 0.0872 us |
| DoubleTest |  74.92 us | 0.3872 us | 0.3622 us |
