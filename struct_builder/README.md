```text
BenchmarkDotNet v0.15.0, Windows 11 (10.0.22631.5413/23H2/2023Update/SunValley3)
12th Gen Intel Core i7-12700 2.10GHz, 1 CPU, 20 logical and 12 physical cores
.NET SDK 9.0.300
[Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2
DefaultJob : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2
```


| Method       | DictionarySize | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0     | Gen1     | Gen2     | Allocated | Alloc Ratio |
|------------- |--------------- |---------:|---------:|---------:|------:|--------:|---------:|---------:|---------:|----------:|------------:|
| WriteDefault | 10             | 12.99 ms | 0.092 ms | 0.082 ms |  1.00 |    0.01 | 500.0000 | 500.0000 | 500.0000 |   7.87 MB |        1.00 |
| WriteP0      | 10             | 12.95 ms | 0.070 ms | 0.066 ms |  1.00 |    0.01 | 500.0000 | 500.0000 | 500.0000 |   7.87 MB |        1.00 |
| WriteP1      | 10             | 12.62 ms | 0.058 ms | 0.054 ms |  0.97 |    0.01 | 500.0000 | 500.0000 | 500.0000 |   6.22 MB |        0.79 |
| WriteClass   | 10             | 14.59 ms | 0.282 ms | 0.325 ms |  1.12 |    0.03 | 968.7500 | 953.1250 | 500.0000 |   9.01 MB |        1.14 |
| WriteRecord  | 10             | 14.34 ms | 0.285 ms | 0.328 ms |  1.10 |    0.03 | 968.7500 | 953.1250 | 500.0000 |   9.01 MB |        1.14 |
|              |                |          |          |          |       |         |          |          |          |           |             |
| WriteDefault | 1000           | 12.80 ms | 0.047 ms | 0.042 ms |  1.00 |    0.00 | 500.0000 | 500.0000 | 500.0000 |   7.87 MB |        1.00 |
| WriteP0      | 1000           | 12.83 ms | 0.077 ms | 0.072 ms |  1.00 |    0.01 | 500.0000 | 500.0000 | 500.0000 |   7.87 MB |        1.00 |
| WriteP1      | 1000           | 12.62 ms | 0.031 ms | 0.024 ms |  0.99 |    0.00 | 500.0000 | 500.0000 | 500.0000 |   6.22 MB |        0.79 |
| WriteClass   | 1000           | 14.26 ms | 0.275 ms | 0.243 ms |  1.11 |    0.02 | 968.7500 | 953.1250 | 500.0000 |   9.01 MB |        1.14 |
| WriteRecord  | 1000           | 14.40 ms | 0.267 ms | 0.274 ms |  1.12 |    0.02 | 968.7500 | 953.1250 | 500.0000 |   9.01 MB |        1.14 |
|              |                |          |          |          |       |         |          |          |          |           |             |
| WriteDefault | 10000          | 12.88 ms | 0.114 ms | 0.106 ms |  1.00 |    0.01 | 484.3750 | 484.3750 | 484.3750 |   7.87 MB |        1.00 |
| WriteP0      | 10000          | 12.71 ms | 0.080 ms | 0.075 ms |  0.99 |    0.01 | 484.3750 | 484.3750 | 484.3750 |   7.87 MB |        1.00 |
| WriteP1      | 10000          | 12.55 ms | 0.061 ms | 0.057 ms |  0.97 |    0.01 | 484.3750 | 484.3750 | 484.3750 |   6.22 MB |        0.79 |
| WriteClass   | 10000          | 18.97 ms | 0.377 ms | 0.981 ms |  1.47 |    0.08 | 906.2500 | 843.7500 | 437.5000 |   9.01 MB |        1.14 |
| WriteRecord  | 10000          | 19.11 ms | 0.379 ms | 1.088 ms |  1.48 |    0.08 | 906.2500 | 843.7500 | 437.5000 |   9.01 MB |        1.14 |

| Method            | DictionarySize | Mean     | Error    | StdDev   | Ratio | Allocated | Alloc Ratio |
|------------------ |--------------- |---------:|---------:|---------:|------:|----------:|------------:|
| ReadDefault       | 10             | 32.54 us | 0.201 us | 0.178 us |  1.00 |         - |          NA |
| ReadP0            | 10             | 32.52 us | 0.154 us | 0.144 us |  1.00 |         - |          NA |
| ReadP1            | 10             | 32.73 us | 0.100 us | 0.093 us |  1.01 |         - |          NA |
| ReadDefaultClass  | 10             | 33.05 us | 0.120 us | 0.107 us |  1.02 |         - |          NA |
| ReadDefaultRecord | 10             | 32.77 us | 0.076 us | 0.072 us |  1.01 |         - |          NA |
|                   |                |          |          |          |       |           |             |
| ReadDefault       | 1000           | 34.65 us | 0.208 us | 0.184 us |  1.00 |         - |          NA |
| ReadP0            | 1000           | 34.66 us | 0.123 us | 0.115 us |  1.00 |       1 B |          NA |
| ReadP1            | 1000           | 34.24 us | 0.116 us | 0.103 us |  0.99 |         - |          NA |
| ReadDefaultClass  | 1000           | 37.54 us | 0.244 us | 0.228 us |  1.08 |         - |          NA |
| ReadDefaultRecord | 1000           | 37.37 us | 0.101 us | 0.090 us |  1.08 |         - |          NA |
|                   |                |          |          |          |       |           |             |
| ReadDefault       | 10000          | 41.61 us | 0.130 us | 0.109 us |  1.00 |         - |          NA |
| ReadP0            | 10000          | 41.52 us | 0.059 us | 0.052 us |  1.00 |       1 B |          NA |
| ReadP1            | 10000          | 41.05 us | 0.074 us | 0.069 us |  0.99 |       1 B |          NA |
| ReadDefaultClass  | 10000          | 51.58 us | 0.346 us | 0.323 us |  1.24 |         - |          NA |
| ReadDefaultRecord | 10000          | 51.71 us | 0.308 us | 0.288 us |  1.24 |         - |          NA |

| Method                 | DictionarySize | Mean         | Error      | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------------------- |--------------- |-------------:|-----------:|-----------:|------:|--------:|----------:|------------:|
| EnumerateDefault       | 10             |     18.72 ns |   0.094 ns |   0.084 ns |  1.00 |    0.01 |         - |          NA |
| EnumerateP0            | 10             |     18.52 ns |   0.084 ns |   0.079 ns |  0.99 |    0.01 |         - |          NA |
| EnumerateP1            | 10             |     18.83 ns |   0.094 ns |   0.088 ns |  1.01 |    0.01 |         - |          NA |
| EnumerateDefaultClass  | 10             |     18.27 ns |   0.134 ns |   0.119 ns |  0.98 |    0.01 |         - |          NA |
| EnumerateDefaultRecord | 10             |     18.17 ns |   0.069 ns |   0.065 ns |  0.97 |    0.01 |         - |          NA |
|                        |                |              |            |            |       |         |           |             |
| EnumerateDefault       | 1000           |  1,712.66 ns |  26.426 ns |  22.067 ns |  1.00 |    0.02 |         - |          NA |
| EnumerateP0            | 1000           |  1,740.45 ns |  29.892 ns |  26.498 ns |  1.02 |    0.02 |         - |          NA |
| EnumerateP1            | 1000           |  1,735.86 ns |  24.705 ns |  23.109 ns |  1.01 |    0.02 |         - |          NA |
| EnumerateDefaultClass  | 1000           |  1,741.02 ns |  30.385 ns |  31.203 ns |  1.02 |    0.02 |         - |          NA |
| EnumerateDefaultRecord | 1000           |  1,719.21 ns |  23.287 ns |  21.783 ns |  1.00 |    0.02 |         - |          NA |
|                        |                |              |            |            |       |         |           |             |
| EnumerateDefault       | 10000          | 16,741.16 ns | 208.280 ns | 194.825 ns |  1.00 |    0.02 |         - |          NA |
| EnumerateP0            | 10000          | 16,700.97 ns | 152.004 ns | 142.185 ns |  1.00 |    0.01 |         - |          NA |
| EnumerateP1            | 10000          | 16,658.28 ns |  91.038 ns |  76.021 ns |  1.00 |    0.01 |         - |          NA |
| EnumerateDefaultClass  | 10000          | 19,134.63 ns | 263.925 ns | 246.876 ns |  1.14 |    0.02 |         - |          NA |
| EnumerateDefaultRecord | 10000          | 19,138.21 ns | 342.831 ns | 491.678 ns |  1.14 |    0.03 |         - |          NA |