# String Pool Benchmark Suite

A .NET 9 solution that compares multiple implementations of a string-to-id/id-to-string pool. It includes:
- High-quality microbenchmarks built with BenchmarkDotNet
- Memory diagnostics (Allocated, Gen0/1/2)
- Exported results in Markdown, HTML, CSV (including per-measurement), and JSON
- A comprehensive unit test suite (56 tests)

This repository is intended to help evaluate trade-offs between lock-based, lock-free, striped/sharded, and concurrent-dictionary based designs for string interning/pooling scenarios.
```text
BenchmarkDotNet v0.15.0, Windows 11 (10.0.26100.5074/24H2/2024Update/HudsonValley)
12th Gen Intel Core i7-12700 2.10GHz, 1 CPU, 20 logical and 12 physical cores
.NET SDK 9.0.304
[Host]     : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2
Job-KJXREE : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2

```

| Method                             | DataSize | ThreadCount | Mean           | Error        | StdDev       | Median         | Gen0      | Gen1      | Allocated    |
|----------------------------------- |--------- |------------ |---------------:|-------------:|-------------:|---------------:|----------:|----------:|-------------:|
| DictionaryLock_Concurrent          | 10000    | 1           |       814.2 us |     15.03 us |     14.06 us |       810.1 us |         - |         - |     668.4 KB |
| StripedSharded_Concurrent          | 10000    | 1           |     1,018.5 us |     11.17 us |     11.47 us |     1,016.5 us |         - |         - |     668.4 KB |
| DictionaryReadwriteLock_Concurrent | 10000    | 1           |     1,091.5 us |     12.58 us |     10.51 us |     1,093.1 us |         - |         - |     668.4 KB |
| StripedSharded_Concurrent          | 10000    | 4           |     1,362.0 us |     26.84 us |     41.78 us |     1,350.5 us |         - |         - |    696.73 KB |
| StatePool_Concurrent               | 10000    | 1           |     1,470.5 us |     55.39 us |    158.02 us |     1,511.5 us |         - |         - |   2923.37 KB |
| StripedSharded_Concurrent          | 10000    | 8           |     1,626.1 us |    137.78 us |    399.72 us |     1,448.0 us |         - |         - |    709.43 KB |
| StripedSharded_Concurrent          | 10000    | 16          |     1,630.8 us |    119.83 us |    345.74 us |     1,468.0 us |         - |         - |    647.37 KB |
| StatePool_Concurrent               | 10000    | 4           |     1,753.6 us |     34.90 us |     96.71 us |     1,737.2 us |         - |         - |    2953.3 KB |
| StatePool_Concurrent               | 10000    | 8           |     1,868.1 us |     37.97 us |    107.09 us |     1,864.8 us |         - |         - |    2961.5 KB |
| StatePool_Concurrent               | 10000    | 16          |     2,047.5 us |     55.80 us |    156.47 us |     2,023.3 us |         - |         - |   2903.05 KB |
| LockFree_Concurrent                | 10000    | 16          |     2,353.2 us |    187.77 us |    541.75 us |     2,139.2 us |         - |         - |     812.3 KB |
| LockFree_Concurrent                | 10000    | 8           |     2,492.4 us |    176.00 us |    507.80 us |     2,277.9 us |         - |         - |     834.2 KB |
| LockFree_Concurrent                | 10000    | 4           |     2,573.7 us |    269.87 us |    787.23 us |     2,093.5 us |         - |         - |    853.28 KB |
| DictionaryLock_Concurrent          | 10000    | 4           |     2,711.9 us |     92.36 us |    255.92 us |     2,646.5 us |         - |         - |    632.38 KB |
| DictionaryLock_Concurrent          | 10000    | 8           |     3,946.4 us |    149.91 us |    434.92 us |     3,946.6 us |         - |         - |    614.49 KB |
| DictionaryReadwriteLock_Concurrent | 10000    | 4           |     4,068.6 us |    427.21 us |  1,259.65 us |     4,200.9 us |         - |         - |    696.73 KB |
| DictionaryLock_Concurrent          | 10000    | 16          |     4,238.6 us |    139.88 us |    408.05 us |     4,286.0 us |         - |         - |    658.88 KB |
| LockFree_Concurrent                | 10000    | 1           |     5,002.2 us |     99.52 us |    148.96 us |     5,024.9 us |         - |         - |    824.67 KB |
| DictionaryReadwriteLock_Concurrent | 10000    | 8           |     7,802.3 us |    731.33 us |  2,156.35 us |     8,001.8 us |         - |         - |    645.65 KB |
| DictionaryReadwriteLock_Concurrent | 10000    | 16          |    11,153.1 us |    950.13 us |  2,801.48 us |    10,717.5 us |         - |         - |    643.34 KB |
| DictionaryLock_Concurrent          | 500000   | 1           |    39,637.7 us |    789.94 us |  1,846.45 us |    38,935.5 us |         - |         - |  24196.72 KB |
| DictionaryReadwriteLock_Concurrent | 500000   | 1           |    54,106.5 us |  1,072.31 us |  1,003.04 us |    54,301.7 us |         - |         - |  24196.72 KB |
| StripedSharded_Concurrent          | 500000   | 16          |    54,673.8 us |  1,073.00 us |  1,317.73 us |    54,875.2 us |         - |         - |  32458.35 KB |
| StripedSharded_Concurrent          | 500000   | 8           |    59,962.3 us |  1,163.79 us |  1,553.63 us |    59,929.4 us |         - |         - |  30382.63 KB |
| StripedSharded_Concurrent          | 500000   | 4           |    63,582.8 us |  1,264.66 us |  1,456.38 us |    63,444.7 us |         - |         - |  32417.16 KB |
| StripedSharded_Concurrent          | 500000   | 1           |    63,837.6 us |  1,266.54 us |  2,251.27 us |    64,110.9 us |         - |         - |  24196.72 KB |
| DictionaryReadwriteLock_Concurrent | 500000   | 4           |    84,834.7 us |  1,680.69 us |  2,714.00 us |    84,775.6 us |         - |         - |  28321.14 KB |
| DictionaryLock_Concurrent          | 500000   | 4           |   103,127.2 us |  2,029.24 us |  3,334.09 us |   102,854.1 us |         - |         - |  28321.14 KB |
| DictionaryLock_Concurrent          | 500000   | 16          |   111,672.3 us |  1,655.22 us |  1,382.18 us |   111,185.2 us |         - |         - |  30409.63 KB |
| DictionaryLock_Concurrent          | 500000   | 8           |   124,693.8 us |  2,491.00 us |  2,208.21 us |   124,579.1 us |         - |         - |   28334.6 KB |
| DictionaryReadwriteLock_Concurrent | 500000   | 8           |   153,795.5 us |  3,766.84 us | 10,868.18 us |   154,161.6 us |         - |         - |  30382.69 KB |
| StatePool_Concurrent               | 500000   | 16          |   181,489.6 us |  5,786.71 us | 16,971.42 us |   182,113.8 us | 6000.0000 | 3000.0000 | 117370.91 KB |
| StatePool_Concurrent               | 500000   | 1           |   185,557.3 us |  3,709.52 us |  5,077.63 us |   185,308.9 us | 6000.0000 | 3000.0000 | 110134.22 KB |
| StatePool_Concurrent               | 500000   | 8           |   198,556.9 us |  4,679.11 us | 13,723.01 us |   200,637.4 us | 6000.0000 | 3000.0000 | 116320.13 KB |
| StatePool_Concurrent               | 500000   | 4           |   198,779.7 us |  3,954.41 us |  5,412.84 us |   199,386.7 us | 6000.0000 | 3000.0000 | 114259.25 KB |
| DictionaryReadwriteLock_Concurrent | 500000   | 16          |   289,546.4 us |  5,759.24 us |  5,656.34 us |   290,302.2 us |         - |         - |  27337.44 KB |
| LockFree_Concurrent                | 500000   | 16          |   842,028.2 us | 23,601.70 us | 68,847.28 us |   858,899.9 us |         - |         - |  38223.34 KB |
| LockFree_Concurrent                | 500000   | 8           |   866,437.7 us | 17,278.15 us | 30,261.29 us |   868,547.5 us |         - |         - |  40244.11 KB |
| LockFree_Concurrent                | 500000   | 4           | 1,188,720.2 us | 21,940.06 us | 20,522.75 us | 1,192,219.4 us |         - |         - |  40230.13 KB |
| LockFree_Concurrent                | 500000   | 1           | 4,557,575.7 us | 72,943.46 us | 81,076.46 us | 4,554,029.5 us |         - |         - |  32009.24 KB |