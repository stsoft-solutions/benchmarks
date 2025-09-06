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
  Job-AAGOZD : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2
```

| Method                  | DataSize | ThreadCount | Mean           | Error        | StdDev        | Median         | Gen0       | Gen1       | Gen2      | Allocated    |
|------------------------ |--------- |------------ |---------------:|-------------:|--------------:|---------------:|-----------:|-----------:|----------:|-------------:|
| DictionaryLock          | 10000    | 1           |       569.8 us |      8.05 us |       6.73 us |       568.1 us |          - |          - |         - |     39.48 KB |
| LockFree                | 10000    | 1           |       424.4 us |      8.37 us |       7.42 us |       424.7 us |          - |          - |         - |     39.15 KB |
| StatePool               | 10000    | 1           |     1,195.1 us |     23.85 us |      41.14 us |     1,191.3 us |          - |          - |         - |   2294.16 KB |
| DictionaryReadwriteLock | 10000    | 1           |       848.1 us |      4.26 us |       3.32 us |       846.4 us |          - |          - |         - |      39.2 KB |
| StripedSharded          | 10000    | 1           |       766.4 us |      4.47 us |       3.74 us |       766.1 us |          - |          - |         - |      39.2 KB |
| DictionaryLock          | 10000    | 4           |     2,144.5 us |    127.69 us |     376.50 us |     2,048.7 us |          - |          - |         - |     45.81 KB |
| LockFree                | 10000    | 4           |       891.2 us |     16.83 us |      29.92 us |       885.0 us |          - |          - |         - |     45.75 KB |
| StatePool               | 10000    | 4           |     1,677.2 us |     51.45 us |     147.63 us |     1,634.3 us |          - |          - |         - |   2304.43 KB |
| DictionaryReadwriteLock | 10000    | 4           |     2,590.0 us |    177.69 us |     523.93 us |     2,542.6 us |          - |          - |         - |     45.81 KB |
| StripedSharded          | 10000    | 4           |     1,220.0 us |     24.23 us |      34.75 us |     1,213.6 us |          - |          - |         - |     45.75 KB |
| DictionaryLock          | 10000    | 8           |     2,776.2 us |     54.94 us |     103.20 us |     2,768.3 us |          - |          - |         - |     47.61 KB |
| LockFree                | 10000    | 8           |       507.3 us |     16.37 us |      44.27 us |       496.2 us |          - |          - |         - |        48 KB |
| StatePool               | 10000    | 8           |     1,829.0 us |     80.58 us |     231.20 us |     1,745.1 us |          - |          - |         - |   2303.79 KB |
| DictionaryReadwriteLock | 10000    | 8           |     5,836.8 us |    415.06 us |   1,223.81 us |     6,040.4 us |          - |          - |         - |        48 KB |
| StripedSharded          | 10000    | 8           |     1,184.9 us |     23.50 us |      55.84 us |     1,172.2 us |          - |          - |         - |     47.61 KB |
| DictionaryLock          | 10000    | 16          |     2,796.7 us |     67.37 us |     193.31 us |     2,793.4 us |          - |          - |         - |      52.5 KB |
| LockFree                | 10000    | 16          |       625.7 us |     36.57 us |     103.74 us |       637.1 us |          - |          - |         - |      48.4 KB |
| StatePool               | 10000    | 16          |     1,577.3 us |     31.50 us |      83.54 us |     1,559.9 us |          - |          - |         - |   2307.22 KB |
| DictionaryReadwriteLock | 10000    | 16          |     8,507.4 us |    489.04 us |   1,441.94 us |     8,672.1 us |          - |          - |         - |     52.17 KB |
| StripedSharded          | 10000    | 16          |     1,293.8 us |     70.41 us |     203.14 us |     1,196.8 us |          - |          - |         - |     52.27 KB |
| DictionaryLock          | 500000   | 1           |    28,465.0 us |    564.42 us |   1,363.13 us |    27,891.2 us |          - |          - |         - |   1953.21 KB |
| LockFree                | 500000   | 1           |   152,143.8 us |  2,738.82 us |   2,427.90 us |   152,378.0 us |          - |          - |         - |   1953.26 KB |
| StatePool               | 500000   | 1           |   159,501.7 us |  2,316.69 us |   1,934.54 us |   158,712.6 us |  6000.0000 |  3000.0000 |         - |  87890.71 KB |
| DictionaryReadwriteLock | 500000   | 1           |    42,097.7 us |    627.59 us |     616.38 us |    42,014.7 us |          - |          - |         - |   1953.21 KB |
| StripedSharded          | 500000   | 1           |    49,263.2 us |    685.31 us |     733.28 us |    49,147.7 us |          - |          - |         - |   1953.21 KB |
| DictionaryLock          | 500000   | 4           |    85,141.4 us |  1,682.66 us |   3,585.88 us |    85,334.4 us |          - |          - |         - |   1959.48 KB |
| LockFree                | 500000   | 4           |    50,235.2 us |  1,224.86 us |   3,533.99 us |    49,917.3 us |          - |          - |         - |   1960.09 KB |
| StatePool               | 500000   | 4           |   177,986.5 us |  3,554.73 us |   9,364.58 us |   179,401.6 us |  6000.0000 |  2000.0000 |         - |  87897.11 KB |
| DictionaryReadwriteLock | 500000   | 4           |    80,030.5 us |  1,592.97 us |   2,384.29 us |    80,403.2 us |          - |          - |         - |   1959.94 KB |
| StripedSharded          | 500000   | 4           |    57,236.6 us |  1,123.06 us |   1,201.66 us |    57,104.8 us |          - |          - |         - |   1960.09 KB |
| DictionaryLock          | 500000   | 8           |    81,515.3 us |  1,621.67 us |   2,524.74 us |    81,993.9 us |          - |          - |         - |   1961.86 KB |
| LockFree                | 500000   | 8           |    50,460.0 us |  1,002.00 us |   2,674.53 us |    50,276.6 us |          - |          - |         - |   1961.73 KB |
| StatePool               | 500000   | 8           |   179,582.9 us |  3,558.01 us |  10,435.03 us |   181,275.9 us |  6000.0000 |  3000.0000 |         - |  87899.23 KB |
| DictionaryReadwriteLock | 500000   | 8           |   123,512.0 us |  2,413.85 us |   3,758.07 us |   123,154.3 us |          - |          - |         - |   1961.73 KB |
| StripedSharded          | 500000   | 8           |    55,979.4 us |  1,118.37 us |   1,196.64 us |    56,093.9 us |          - |          - |         - |    1961.8 KB |
| DictionaryLock          | 500000   | 16          |    79,780.6 us |  1,571.15 us |   3,173.79 us |    79,914.1 us |          - |          - |         - |    1966.3 KB |
| LockFree                | 500000   | 16          |    38,077.6 us |  1,131.23 us |   3,190.65 us |    37,826.3 us |          - |          - |         - |   1966.13 KB |
| StatePool               | 500000   | 16          |   186,528.5 us |  3,979.47 us |  11,733.55 us |   187,732.1 us |  6000.0000 |  3000.0000 |         - |  87903.67 KB |
| DictionaryReadwriteLock | 500000   | 16          |   197,199.5 us |  3,909.52 us |   7,049.67 us |   196,409.5 us |          - |          - |         - |   1966.17 KB |
| StripedSharded          | 500000   | 16          |    51,701.7 us |    837.61 us |     997.11 us |    51,999.6 us |          - |          - |         - |   1966.17 KB |
| DictionaryLock          | 2000000  | 1           |   156,573.0 us |  2,433.91 us |   3,249.20 us |   156,558.5 us |          - |          - |         - |   7812.91 KB |
| LockFree                | 2000000  | 1           | 1,046,559.4 us | 11,556.72 us |  10,810.16 us | 1,046,312.0 us |          - |          - |         - |   7812.59 KB |
| StatePool               | 2000000  | 1           | 1,069,705.9 us | 14,024.39 us |  12,432.26 us | 1,070,754.2 us | 26000.0000 | 13000.0000 |         - | 351562.59 KB |
| DictionaryReadwriteLock | 2000000  | 1           |   210,726.3 us |  3,195.98 us |   2,668.79 us |   210,082.4 us |          - |          - |         - |   7812.59 KB |
| StripedSharded          | 2000000  | 1           |   349,107.1 us |  6,382.46 us |   5,657.89 us |   348,348.0 us |          - |          - |         - |   7812.59 KB |
| DictionaryLock          | 2000000  | 4           |   361,054.4 us |  6,932.77 us |   6,808.91 us |   362,441.6 us |          - |          - |         - |   7818.98 KB |
| LockFree                | 2000000  | 4           |   346,147.9 us |  6,881.66 us |  15,391.81 us |   345,403.8 us |          - |          - |         - |   7818.98 KB |
| StatePool               | 2000000  | 4           | 1,091,444.3 us | 21,423.42 us |  20,039.48 us | 1,096,120.9 us | 26000.0000 | 13000.0000 |         - |  351569.9 KB |
| DictionaryReadwriteLock | 2000000  | 4           |   381,090.9 us |  6,372.32 us |   5,648.90 us |   380,778.2 us |          - |          - |         - |   7818.86 KB |
| StripedSharded          | 2000000  | 4           |   263,624.4 us |  2,554.34 us |   2,132.99 us |   263,640.0 us |          - |          - |         - |   7818.86 KB |
| DictionaryLock          | 2000000  | 8           |   356,706.7 us |  6,963.17 us |   8,018.80 us |   355,344.2 us |          - |          - |         - |   7821.05 KB |
| LockFree                | 2000000  | 8           |   298,974.5 us |  5,973.15 us |  13,111.21 us |   298,280.5 us |          - |          - |         - |   7822.13 KB |
| StatePool               | 2000000  | 8           | 1,035,158.3 us | 37,893.15 us | 111,728.82 us | 1,093,715.8 us | 28000.0000 | 14000.0000 | 1000.0000 | 351590.46 KB |
| DictionaryReadwriteLock | 2000000  | 8           |   726,752.0 us | 14,419.09 us |  40,904.55 us |   733,427.8 us |          - |          - |         - |   7821.78 KB |
| StripedSharded          | 2000000  | 8           |   241,224.6 us |  4,175.25 us |   3,486.52 us |   240,973.4 us |          - |          - |         - |   7821.23 KB |
| DictionaryLock          | 2000000  | 16          |   377,935.7 us |  4,677.25 us |   4,146.26 us |   379,003.6 us |          - |          - |         - |   7826.09 KB |
| LockFree                | 2000000  | 16          |   256,023.8 us |  4,908.15 us |  11,277.30 us |   255,027.9 us |          - |          - |         - |   7825.48 KB |
| StatePool               | 2000000  | 16          |   954,912.5 us | 27,656.82 us |  81,546.76 us |   936,591.7 us | 28000.0000 | 14000.0000 | 1000.0000 | 351673.66 KB |
| DictionaryReadwriteLock | 2000000  | 16          |   873,693.1 us | 11,643.73 us |  12,941.98 us |   872,634.0 us |          - |          - |         - |   7827.26 KB |
| StripedSharded          | 2000000  | 16          |   203,833.7 us |  1,900.14 us |   1,586.70 us |   203,821.1 us |          - |          - |         - |   7826.16 KB |