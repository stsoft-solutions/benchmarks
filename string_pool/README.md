BenchmarkDotNet v0.15.0, Windows 11 (10.0.22631.5335/23H2/2023Update/SunValley3)</br>
12th Gen Intel Core i7-12700 2.10GHz, 1 CPU, 20 logical and 12 physical cores </br>
.NET SDK 9.0.300
- [Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2
- DefaultJob : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX2

<div style="overflow-x: auto">
  <pre>
| Method                | DataSize | Mean         | Error      | StdDev       | Median       | Gen0      | Gen1      | Gen2      | Allocated   |
|---------------------- |--------- |-------------:|-----------:|-------------:|-------------:|----------:|----------:|----------:|------------:|
| Lock_SingleThread     | 1000     |     61.15 us |   0.955 us |     0.847 us |     60.94 us |   16.4795 |   16.3574 |    0.2441 |   211.94 KB |
| RwLock_SingleThread   | 1000     |     88.86 us |   0.696 us |     0.617 us |     88.77 us |   16.4795 |   16.3574 |    0.2441 |   211.99 KB |
| LockFree_SingleThread | 1000     |    118.81 us |   0.571 us |     0.506 us |    118.59 us |   29.2969 |   29.1748 |    0.3662 |   375.26 KB |
| Lock_MultiThread      | 1000     |    521.71 us |  10.111 us |     9.458 us |    517.94 us |   22.4609 |   21.4844 |         - |   278.16 KB |
| RwLock_MultiThread    | 1000     |    763.90 us |  20.789 us |    60.970 us |    730.47 us |   19.5313 |   17.5781 |         - |   249.86 KB |
| LockFree_MultiThread  | 1000     |    317.20 us |  16.990 us |    50.097 us |    280.46 us |   31.2500 |   30.7617 |    0.4883 |   387.52 KB |
| Lock_SingleThread     | 50000    |  4,090.14 us |  44.932 us |    39.831 us |  4,093.66 us | 1234.3750 | 1203.1250 | 1195.3125 |   8661.8 KB |
| RwLock_SingleThread   | 50000    |  5,419.45 us |  30.462 us |    25.437 us |  5,428.28 us | 1343.7500 | 1312.5000 | 1304.6875 |  8661.53 KB |
| LockFree_SingleThread | 50000    | 12,651.12 us | 250.934 us |   581.577 us | 12,589.18 us | 1296.8750 | 1296.8750 |  656.2500 | 10993.65 KB |
| Lock_MultiThread      | 50000    | 13,096.61 us | 197.401 us |   174.991 us | 13,088.35 us | 1187.5000 | 1125.0000 | 1093.7500 |  8831.48 KB |
| RwLock_MultiThread    | 50000    | 24,786.19 us | 346.848 us |   324.441 us | 24,692.08 us | 1000.0000 |  937.5000 |  906.2500 |  8797.66 KB |
| LockFree_MultiThread  | 50000    | 20,353.78 us | 404.557 us | 1,065.765 us | 20,363.29 us | 1281.2500 | 1093.7500 |  625.0000 | 11285.92 KB |
| Lock_SingleThread     | 100000   |  9,733.72 us | 193.993 us |   307.693 us |  9,792.57 us |  984.3750 |  953.1250 |  953.1250 | 17925.07 KB |
| RwLock_SingleThread   | 100000   | 12,118.24 us | 142.898 us |   133.667 us | 12,126.00 us |  937.5000 |  906.2500 |  906.2500 | 17924.12 KB |
| LockFree_SingleThread | 100000   | 38,724.59 us | 834.201 us | 2,459.662 us | 39,703.05 us | 2416.6667 | 2333.3333 | 1083.3333 | 23168.35 KB |
| Lock_MultiThread      | 100000   | 26,469.56 us | 524.121 us |   514.757 us | 26,542.32 us |  812.5000 |  750.0000 |  687.5000 |  18150.4 KB |
| RwLock_MultiThread    | 100000   | 52,167.25 us | 911.429 us | 1,335.962 us | 51,945.60 us | 1000.0000 |  888.8889 |  888.8889 | 18107.65 KB |
| LockFree_MultiThread  | 100000   | 42,996.19 us | 853.257 us | 2,461.843 us | 43,140.29 us | 2307.6923 | 2230.7692 |  923.0769 |  23501.4 KB |
  </pre>
</div>

- DataSize: Value of the 'DataSize' parameter
- Mean: Arithmetic mean of all measurements
- Error: Half of 99.9% confidence interval
- StdDev    : Standard deviation of all measurements
- Gen0: GC Generation 0 collects per 1000 operations
- Gen1: GC Generation 1 collects per 1000 operations
- Gen2: GC Generation 2 collects per 1000 operations
- Allocated: Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
- 1 us: 1 Microsecond (0.000001 sec)
