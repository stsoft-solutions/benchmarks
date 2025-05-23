BenchmarkDotNet v0.15.0, Windows 11 (10.0.22631.5335/23H2/2023Update/SunValley3)</br>
12th Gen Intel Core i7-12700 2.10GHz, 1 CPU, 20 logical and 12 physical cores </br>
.NET SDK 9.0.300
- [Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2
- DefaultJob : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2


| Method                | DataSize | Mean         | Error      | StdDev     | Gen0      | Gen1     | Gen2     | Allocated  |
|---------------------- |--------- |-------------:|-----------:|-----------:|----------:|---------:|---------:|-----------:|
| Lock_SingleThread     | 1000     |     48.55 us |   0.227 us |   0.201 us |         - |        - |        - |          - |                                                                                                                                                                                          
| RwLock_SingleThread   | 1000     |     57.56 us |   0.179 us |   0.168 us |         - |        - |        - |          - |                                                                                                                                                                                          
| LockFree_SingleThread | 1000     |     30.51 us |   0.141 us |   0.132 us |    7.3242 |        - |        - |    96000 B |                                                                                                                                                                                          
| Lock_MultiThread      | 1000     |    516.58 us |   7.939 us |   7.426 us |   12.6953 |  11.7188 |   0.9766 |   149270 B |                                                                                                                                                                                          
| RwLock_MultiThread    | 1000     |    490.50 us |   9.679 us |  17.205 us |    8.7891 |   7.8125 |        - |   107853 B |                                                                                                                                                                                          
| LockFree_MultiThread  | 1000     |     92.99 us |   1.778 us |   1.826 us |   17.8223 |  17.7002 |   1.2207 |   213433 B |                                                                                                                                                                                          
| Lock_SingleThread     | 50000    |  2,555.49 us |   8.596 us |   7.178 us |         - |        - |        - |          - |
| RwLock_SingleThread   | 50000    |  2,984.17 us |  22.957 us |  21.474 us |         - |        - |        - |          - |
| LockFree_SingleThread | 50000    |  2,001.82 us |  26.219 us |  25.750 us |  367.1875 |        - |        - |  4800000 B |
| Lock_MultiThread      | 50000    | 14,886.69 us |  84.433 us |  65.920 us |  250.0000 | 218.7500 | 156.2500 |  3240979 B |
| RwLock_MultiThread    | 50000    | 13,389.44 us | 144.048 us | 127.695 us |  250.0000 | 218.7500 |  62.5000 |  3241272 B |
| LockFree_MultiThread  | 50000    |  4,901.87 us |  95.167 us | 148.163 us |  820.3125 | 539.0625 | 320.3125 |  8011457 B |
| Lock_SingleThread     | 100000   |  5,189.84 us |  18.803 us |  15.701 us |         - |        - |        - |        1 B |                                                                                                                                                                                          
| RwLock_SingleThread   | 100000   |  6,181.07 us |  19.107 us |  14.917 us |         - |        - |        - |        1 B |                                                                                                                                                                                          
| LockFree_SingleThread | 100000   |  4,895.06 us |  54.762 us |  48.545 us |  734.3750 |        - |        - |  9600001 B |                                                                                                                                                                                          
| Lock_MultiThread      | 100000   | 26,324.40 us | 189.232 us | 177.008 us |  375.0000 | 343.7500 | 156.2500 |  6322028 B |                                                                                                                                                                                          
| RwLock_MultiThread    | 100000   | 27,036.06 us | 192.933 us | 385.309 us |  285.7143 | 214.2857 |        - |  6316786 B |                                                                                                                                                                                          
| LockFree_MultiThread  | 100000   |  6,963.50 us | 179.780 us | 515.822 us | 1210.9375 | 648.4375 | 328.1250 | 15786643 B |

- DataSize: Value of the 'DataSize' parameter
- Mean: Arithmetic mean of all measurements
- Error: Half of 99.9% confidence interval
- StdDev    : Standard deviation of all measurements
- Gen0: GC Generation 0 collects per 1000 operations
- Gen1: GC Generation 1 collects per 1000 operations
- Gen2: GC Generation 2 collects per 1000 operations
- Allocated: Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
- 1 us: 1 Microsecond (0.000001 sec)
