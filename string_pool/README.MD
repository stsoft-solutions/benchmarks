BenchmarkDotNet v0.15.0, Windows 11 (10.0.22631.5335/23H2/2023Update/SunValley3)</br>
12th Gen Intel Core i7-12700 2.10GHz, 1 CPU, 20 logical and 12 physical cores </br>
.NET SDK 9.0.300
- [Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2
- DefaultJob : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2


| Method                | DataSize | Mean         | Error        | StdDev       | Median       | Gen0      | Gen1      | Gen2      | Allocated   |
|---------------------- |--------- |-------------:|-------------:|-------------:|-------------:|----------:|----------:|----------:|------------:|
| Lock_SingleThread     | 1000     |     68.28 us |     0.385 us |     0.321 us |     68.19 us |   15.6250 |    5.1270 |         - |   199.73 KB |                                                                                                                                                                    
| RwLock_SingleThread   | 1000     |    101.22 us |     0.451 us |     0.422 us |    101.20 us |   15.6250 |    5.1270 |         - |   199.78 KB |                                                                                                                                                                    
| LockFree_SingleThread | 1000     |    139.28 us |     1.124 us |     0.996 us |    139.14 us |   42.9688 |   18.5547 |         - |   550.55 KB |                                                                                                                                                                    
| Lock_MultiThread      | 1000     |    612.10 us |    12.162 us |    15.381 us |    608.19 us |   17.5781 |    5.8594 |         - |   212.72 KB |                                                                                                                                                                    
| RwLock_MultiThread    | 1000     |    928.41 us |    17.773 us |    43.930 us |    913.26 us |   17.5781 |    5.8594 |         - |   212.19 KB |                                                                                                                                                                    
| LockFree_MultiThread  | 1000     |    294.02 us |     4.513 us |     4.001 us |    293.74 us |   41.5039 |   19.0430 |         - |   519.97 KB |                                                                                                                                                                    
| Lock_SingleThread     | 50000    |  4,767.96 us |    64.076 us |    59.936 us |  4,772.36 us | 1203.1250 | 1179.6875 | 1171.8750 |  7953.72 KB |
| RwLock_SingleThread   | 50000    |  6,325.02 us |    65.044 us |    60.843 us |  6,319.86 us | 1218.7500 | 1187.5000 | 1187.5000 |  7953.73 KB |
| LockFree_SingleThread | 50000    | 22,763.49 us |   417.772 us |   390.784 us | 22,730.35 us | 1843.7500 | 1406.2500 |  562.5000 | 19659.88 KB |
| Lock_MultiThread      | 50000    | 14,526.15 us |   255.885 us |   226.835 us | 14,504.78 us | 1218.7500 | 1187.5000 | 1187.5000 |  7968.44 KB |
| RwLock_MultiThread    | 50000    | 32,372.60 us |   495.316 us |   439.085 us | 32,516.62 us | 1250.0000 | 1250.0000 | 1250.0000 |  7971.54 KB |
| LockFree_MultiThread  | 50000    | 27,865.81 us |   543.298 us |   813.183 us | 27,827.66 us | 1875.0000 | 1343.7500 |  500.0000 |    19778 KB |
| Lock_SingleThread     | 100000   | 10,831.96 us |   135.956 us |   127.173 us | 10,791.97 us |  859.3750 |  828.1250 |  828.1250 | 16508.82 KB |                                                                                                                                                                    
| RwLock_SingleThread   | 100000   | 13,697.23 us |   203.691 us |   190.532 us | 13,698.14 us | 1281.2500 | 1250.0000 | 1250.0000 | 16509.01 KB |                                                                                                                                                                    
| LockFree_SingleThread | 100000   | 57,322.96 us | 1,029.087 us |   962.609 us | 57,090.21 us | 3888.8889 | 3000.0000 | 1111.1111 | 40502.58 KB |                                                                                                                                                                    
| Lock_MultiThread      | 100000   | 28,990.02 us |   551.114 us |   565.953 us | 28,979.25 us | 1750.0000 | 1718.7500 | 1718.7500 | 16529.16 KB |                                                                                                                                                                    
| RwLock_MultiThread    | 100000   | 60,530.40 us | 1,181.396 us | 2,099.930 us | 60,379.01 us | 1000.0000 | 1000.0000 | 1000.0000 | 16522.38 KB |                                                                                                                                                                    
| LockFree_MultiThread  | 100000   | 65,532.38 us | 1,285.618 us | 2,476.950 us | 65,551.87 us | 3750.0000 | 2750.0000 | 1000.0000 | 40690.07 KB |   

- DataSize: Value of the 'DataSize' parameter
- Mean: Arithmetic mean of all measurements
- Error: Half of 99.9% confidence interval
- StdDev    : Standard deviation of all measurements
- Gen0: GC Generation 0 collects per 1000 operations
- Gen1: GC Generation 1 collects per 1000 operations
- Gen2: GC Generation 2 collects per 1000 operations
- Allocated: Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
- 1 us: 1 Microsecond (0.000001 sec)
