# String Pool Benchmark Suite
This benchmark suite compares the performance of various concurrent dictionary implementations used in a string pool. The implementations tested include:
- **DictionaryLock**: A standard dictionary protected by a lock.
- **DictionaryReadwriteLock**: A standard dictionary protected by a read-write lock.
- **StripedSharded**: A dictionary implementation that uses sharding to reduce contention.
- **LockFree**: A lock-free dictionary implementation.
- **StatePool**: A custom implementation that uses a pool of states for managing concurrency.

```text
BenchmarkDotNet v0.15.0, Windows 11 (10.0.26100.5074/24H2/2024Update/HudsonValley)
12th Gen Intel Core i7-12700 2.10GHz, 1 CPU, 20 logical and 12 physical cores
.NET SDK 9.0.304
  [Host]     : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2
  Job-ZZGVXW : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2
```

| Method                  | DataSize | ThreadCount | Mean         | Error       | StdDev      | Median       | Gen0      | Gen1      | Allocated    |
|------------------------ |--------- |------------ |-------------:|------------:|------------:|-------------:|----------:|----------:|-------------:|
| LockFree                | 10000    | 1           |     631.3 us |    11.49 us |    10.19 us |     630.7 us |         - |         - |     668.4 KB |
| DictionaryLock          | 10000    | 1           |     748.2 us |    14.84 us |    15.24 us |     742.8 us |         - |         - |    668.68 KB |
| LockFree                | 10000    | 8           |     882.5 us |    17.58 us |    43.78 us |     871.3 us |         - |         - |    613.64 KB |
| LockFree                | 10000    | 16          |     958.1 us |    19.10 us |    38.59 us |     949.6 us |         - |         - |    704.29 KB |
| StripedSharded          | 10000    | 1           |     992.1 us |    17.28 us |    15.31 us |     991.9 us |         - |         - |     668.4 KB |
| DictionaryReadwriteLock | 10000    | 1           |   1,058.2 us |    20.98 us |    21.55 us |   1,051.2 us |         - |         - |     668.4 KB |
| LockFree                | 10000    | 4           |   1,072.2 us |    76.10 us |   220.77 us |     960.0 us |         - |         - |     696.4 KB |
| StatePool               | 10000    | 1           |   1,319.2 us |    55.08 us |   156.24 us |   1,341.8 us |         - |         - |   2923.37 KB |
| StripedSharded          | 10000    | 16          |   1,455.7 us |    33.95 us |    95.75 us |   1,448.5 us |         - |         - |    687.17 KB |
| StripedSharded          | 10000    | 8           |   1,554.8 us |   127.82 us |   368.78 us |   1,389.4 us |         - |         - |    709.65 KB |
| StripedSharded          | 10000    | 4           |   1,574.7 us |   133.31 us |   382.49 us |   1,398.0 us |         - |         - |    696.73 KB |
| StatePool               | 10000    | 4           |   1,752.1 us |    34.76 us |    93.38 us |   1,739.8 us |         - |         - |   2953.72 KB |
| StatePool               | 10000    | 16          |   1,923.3 us |    55.36 us |   157.95 us |   1,898.6 us |         - |         - |   2950.59 KB |
| StatePool               | 10000    | 8           |   1,968.4 us |    39.06 us |    98.70 us |   1,958.7 us |         - |         - |   2934.54 KB |
| DictionaryLock          | 10000    | 4           |   2,762.1 us |    52.80 us |    54.22 us |   2,758.0 us |         - |         - |     632.7 KB |
| DictionaryLock          | 10000    | 8           |   3,587.5 us |   117.94 us |   342.17 us |   3,646.3 us |         - |         - |    689.52 KB |
| DictionaryLock          | 10000    | 16          |   3,633.4 us |    72.52 us |   110.74 us |   3,602.8 us |         - |         - |    659.67 KB |
| DictionaryReadwriteLock | 10000    | 4           |   4,241.5 us |   495.56 us | 1,461.18 us |   4,137.9 us |         - |         - |    696.73 KB |
| DictionaryReadwriteLock | 10000    | 8           |   7,558.4 us |   588.60 us | 1,735.50 us |   7,556.6 us |         - |         - |    709.76 KB |
| DictionaryReadwriteLock | 10000    | 16          |  13,808.7 us |   749.73 us | 2,210.59 us |  13,085.6 us |         - |         - |    735.18 KB |
| LockFree                | 500000   | 16          |  36,718.9 us |   725.52 us | 1,681.51 us |  36,341.4 us |         - |         - |  29129.39 KB |
| DictionaryLock          | 500000   | 1           |  38,486.0 us |   768.35 us | 1,461.87 us |  37,857.7 us |         - |         - |  24196.72 KB |
| DictionaryReadwriteLock | 500000   | 1           |  52,972.1 us | 1,054.28 us | 1,443.10 us |  52,649.7 us |         - |         - |  24196.72 KB |
| StripedSharded          | 500000   | 16          |  55,182.7 us |   939.62 us | 1,044.38 us |  55,130.1 us |         - |         - |  27337.38 KB |
| LockFree                | 500000   | 8           |  57,611.0 us | 1,075.14 us | 1,055.93 us |  57,346.6 us |         - |         - |  34478.67 KB |
| StripedSharded          | 500000   | 8           |  59,811.5 us | 1,189.24 us | 1,272.47 us |  59,951.2 us |         - |         - |  30382.63 KB |
| StripedSharded          | 500000   | 1           |  63,339.5 us | 1,206.88 us | 1,239.38 us |  62,844.6 us |         - |         - |  24197.05 KB |
| StripedSharded          | 500000   | 4           |  63,946.5 us | 1,226.48 us | 1,024.16 us |  63,971.3 us |         - |         - |  24225.73 KB |
| LockFree                | 500000   | 4           |  67,280.5 us | 1,336.58 us | 1,312.70 us |  67,428.4 us |         - |         - |  32417.23 KB |
| DictionaryReadwriteLock | 500000   | 4           |  83,856.8 us | 1,674.26 us | 3,019.03 us |  83,784.9 us |         - |         - |  24225.12 KB |
| DictionaryLock          | 500000   | 4           | 107,626.9 us | 1,733.08 us | 2,063.11 us | 107,252.8 us |         - |         - |  32417.16 KB |
| DictionaryLock          | 500000   | 16          | 109,490.2 us | 2,061.31 us | 3,386.79 us | 109,066.8 us |         - |         - |  29385.55 KB |
| DictionaryLock          | 500000   | 8           | 124,928.5 us | 2,470.14 us | 2,940.53 us | 125,315.1 us |         - |         - |  32430.65 KB |
| DictionaryReadwriteLock | 500000   | 8           | 143,577.2 us | 3,352.17 us | 9,725.25 us | 141,743.5 us |         - |         - |  24238.55 KB |
| LockFree                | 500000   | 1           | 171,681.2 us | 3,153.32 us | 2,949.62 us | 172,648.7 us |         - |         - |  24196.72 KB |
| StatePool               | 500000   | 16          | 175,488.3 us | 3,469.46 us | 5,602.52 us | 174,413.5 us | 6000.0000 | 3000.0000 | 115578.93 KB |
| StatePool               | 500000   | 1           | 179,695.4 us | 3,341.62 us | 3,125.75 us | 178,727.7 us | 6000.0000 | 3000.0000 | 110134.55 KB |
| StatePool               | 500000   | 8           | 181,716.8 us | 3,266.28 us | 4,787.67 us | 179,919.7 us | 6000.0000 | 3000.0000 | 118368.15 KB |
| StatePool               | 500000   | 4           | 184,396.2 us | 3,547.99 us | 4,736.47 us | 183,404.1 us | 6000.0000 | 3000.0000 | 114258.64 KB |
| DictionaryReadwriteLock | 500000   | 16          | 234,172.8 us | 4,500.79 us | 4,210.04 us | 234,615.5 us |         - |         - |  27337.44 KB |