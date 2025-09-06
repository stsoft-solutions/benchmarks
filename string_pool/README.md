<style>
    table {
        width: 100%;
    }
</style>

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
  Job-UEODAN : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2
```

| Method                             | DataSize | ThreadCount | Mean           | Error        | StdDev       | Median         | Gen0      | Gen1      | Allocated    |
|----------------------------------- |--------- |------------ |---------------:|-------------:|-------------:|---------------:|----------:|----------:|-------------:|
| DictionaryLock_Concurrent          | 10000    | 1           |       783.9 us |     14.67 us |     13.72 us |       782.6 us |         - |         - |    668.68 KB |
| StripedSharded_Concurrent          | 10000    | 1           |     1,016.8 us |     18.55 us |     24.77 us |     1,013.8 us |         - |         - |     668.4 KB |
| DictionaryReadwriteLock_Concurrent | 10000    | 1           |     1,074.4 us |     18.93 us |     21.04 us |     1,066.5 us |         - |         - |     668.4 KB |
| StripedSharded_Concurrent          | 10000    | 4           |     1,327.5 us |     25.76 us |     45.11 us |     1,318.7 us |         - |         - |    696.68 KB |
| StripedSharded_Concurrent          | 10000    | 8           |     1,522.5 us |    139.38 us |    402.14 us |     1,330.5 us |         - |         - |     709.7 KB |
| StatePool_Concurrent               | 10000    | 1           |     1,532.2 us |     30.48 us |     29.93 us |     1,531.9 us |         - |         - |   2923.37 KB |
| StripedSharded_Concurrent          | 10000    | 16          |     1,566.6 us |    107.37 us |    308.08 us |     1,420.9 us |         - |         - |    615.66 KB |
| StatePool_Concurrent               | 10000    | 4           |     1,807.8 us |     57.43 us |    164.79 us |     1,762.1 us |         - |         - |   2952.16 KB |
| StatePool_Concurrent               | 10000    | 8           |     1,850.0 us |     46.10 us |    131.51 us |     1,813.3 us |         - |         - |   2962.56 KB |
| StatePool_Concurrent               | 10000    | 16          |     1,969.3 us |     40.15 us |    111.93 us |     1,970.9 us |         - |         - |   2966.98 KB |
| LockFree_Concurrent                | 10000    | 16          |     2,582.6 us |    202.43 us |    587.30 us |     2,309.9 us |         - |         - |    828.51 KB |
| DictionaryLock_Concurrent          | 10000    | 4           |     2,805.9 us |     91.52 us |    241.09 us |     2,755.5 us |         - |         - |    632.72 KB |
| LockFree_Concurrent                | 10000    | 4           |     2,928.0 us |    252.33 us |    736.07 us |     2,560.9 us |         - |         - |       853 KB |
| LockFree_Concurrent                | 10000    | 8           |     3,247.3 us |    240.56 us |    701.73 us |     3,353.1 us |         - |         - |    834.11 KB |
| DictionaryLock_Concurrent          | 10000    | 16          |     3,415.4 us |     66.02 us |     76.03 us |     3,441.5 us |         - |         - |    694.55 KB |
| DictionaryReadwriteLock_Concurrent | 10000    | 4           |     3,856.8 us |    549.19 us |  1,619.29 us |     3,033.1 us |         - |         - |    697.07 KB |
| DictionaryLock_Concurrent          | 10000    | 8           |     3,946.0 us |    132.27 us |    385.84 us |     4,021.3 us |         - |         - |    633.61 KB |
| LockFree_Concurrent                | 10000    | 1           |     6,079.1 us |    116.61 us |    129.61 us |     6,055.0 us |         - |         - |    824.67 KB |
| DictionaryReadwriteLock_Concurrent | 10000    | 8           |     7,594.7 us |    654.29 us |  1,929.18 us |     7,857.6 us |         - |         - |    677.86 KB |
| DictionaryReadwriteLock_Concurrent | 10000    | 16          |    10,628.1 us |    941.25 us |  2,654.82 us |    10,073.0 us |         - |         - |    687.49 KB |
| DictionaryLock_Concurrent          | 500000   | 1           |    38,197.4 us |    708.04 us |  1,430.27 us |    37,836.0 us |         - |         - |  24197.05 KB |
| DictionaryReadwriteLock_Concurrent | 500000   | 1           |    51,381.9 us |    654.02 us |    642.34 us |    51,524.4 us |         - |         - |  24196.72 KB |
| StripedSharded_Concurrent          | 500000   | 16          |    56,211.8 us |  1,111.75 us |  1,365.33 us |    56,041.2 us |         - |         - |  29385.61 KB |
| StripedSharded_Concurrent          | 500000   | 8           |    58,371.2 us |  1,154.08 us |  1,185.16 us |    58,396.9 us |         - |         - |   28334.6 KB |
| StripedSharded_Concurrent          | 500000   | 1           |    58,788.0 us |  1,174.91 us |    981.10 us |    59,021.1 us |         - |         - |  24197.05 KB |
| StripedSharded_Concurrent          | 500000   | 4           |    63,721.1 us |  1,245.72 us |  1,279.27 us |    63,522.4 us |         - |         - |  28321.75 KB |
| DictionaryReadwriteLock_Concurrent | 500000   | 4           |    81,513.0 us |  1,341.48 us |  1,491.05 us |    81,147.1 us |         - |         - |  28321.14 KB |
| DictionaryLock_Concurrent          | 500000   | 4           |   103,907.6 us |  2,046.41 us |  3,062.97 us |   103,842.6 us |         - |         - |  28321.14 KB |
| DictionaryLock_Concurrent          | 500000   | 16          |   106,449.9 us |  2,068.85 us |  2,831.87 us |   106,573.7 us |         - |         - |  31433.53 KB |
| DictionaryLock_Concurrent          | 500000   | 8           |   115,511.7 us |  2,252.15 us |  2,681.03 us |   115,284.5 us |         - |         - |  30382.81 KB |
| DictionaryReadwriteLock_Concurrent | 500000   | 8           |   133,563.0 us |  2,667.34 us |  7,026.83 us |   131,638.4 us |         - |         - |   28334.6 KB |
| StatePool_Concurrent               | 500000   | 16          |   166,602.5 us |  4,012.38 us | 11,767.61 us |   168,662.4 us | 6000.0000 | 3000.0000 | 115323.23 KB |
| StatePool_Concurrent               | 500000   | 1           |   168,250.7 us |  3,345.74 us |  4,350.41 us |   166,875.0 us | 6000.0000 | 3000.0000 | 110134.22 KB |
| StatePool_Concurrent               | 500000   | 8           |   184,287.1 us |  3,760.95 us | 11,030.21 us |   185,479.9 us | 6000.0000 | 3000.0000 | 114272.29 KB |
| StatePool_Concurrent               | 500000   | 4           |   193,618.6 us |  3,865.53 us | 10,384.48 us |   196,225.2 us | 6000.0000 | 3000.0000 | 110162.62 KB |
| DictionaryReadwriteLock_Concurrent | 500000   | 16          |   254,718.6 us |  5,089.49 us | 11,996.53 us |   256,658.3 us |         - |         - |  27337.38 KB |
| LockFree_Concurrent                | 500000   | 16          |   671,468.9 us | 12,733.84 us | 12,506.34 us |   672,706.9 us |         - |         - |  39246.93 KB |
| LockFree_Concurrent                | 500000   | 8           | 1,070,349.4 us | 21,328.70 us | 54,673.61 us | 1,070,721.7 us |         - |         - |  42291.98 KB |
| LockFree_Concurrent                | 500000   | 4           | 1,878,013.9 us | 23,605.25 us | 22,080.37 us | 1,870,702.8 us |         - |         - |   40230.5 KB |
| LockFree_Concurrent                | 500000   | 1           | 5,807,125.6 us | 20,390.92 us | 17,027.34 us | 5,808,575.3 us |         - |         - |  32009.52 KB |