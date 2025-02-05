# Chunk enumeration
Split a IEnumerable<T> into chunks of IEnumerable<T> with a specified size.

Chunk size = 50;

- ChunkCore - .Net implementation with T[] Chunk()
- ChunkLigh - use a special structure to manage an IEnumerable
- Chunk - simplest implementation with yield.

## Results
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2894)<br>
12th Gen Intel Core i7-1255U, 1 CPU, 12 logical and 10 physical cores<br>
.NET SDK 9.0.102<br>
  [Host]     : .NET 9.0.1 (9.0.124.61010), X64 RyuJIT AVX2<br>
  DefaultJob : .NET 9.0.1 (9.0.124.61010), X64 RyuJIT AVX2<br>


| Method     | NumberOfElements | Mean          | Error        | StdDev       | Gen0    | Allocated |
|----------- |----------------- |--------------:|-------------:|-------------:|--------:|----------:|
| Chunk      | 10               |      41.71 ns |     0.848 ns |     1.508 ns |  0.0255 |     160 B |
| ChunkLight | 10               |      14.96 ns |     0.279 ns |     0.400 ns |  0.0063 |      40 B |
| ChunkCore  | 10               |      59.15 ns |     1.187 ns |     1.813 ns |  0.0573 |     360 B |
| Chunk      | 100              |     225.95 ns |     4.104 ns |     6.143 ns |  0.0343 |     216 B |
| ChunkLight | 100              |     124.05 ns |     2.376 ns |     2.641 ns |  0.0062 |      40 B |
| ChunkCore  | 100              |     283.18 ns |     5.548 ns |     5.936 ns |  0.1426 |     896 B |
| Chunk      | 500              |   1,076.43 ns |    19.591 ns |    20.119 ns |  0.1049 |     664 B |
| ChunkLight | 500              |     589.25 ns |    11.721 ns |    13.028 ns |  0.0057 |      40 B |
| ChunkCore  | 500              |   1,107.66 ns |    21.822 ns |    38.788 ns |  0.4272 |    2688 B |
| Chunk      | 1000             |   2,104.42 ns |    36.721 ns |    34.349 ns |  0.1945 |    1224 B |
| ChunkLight | 1000             |   1,190.37 ns |    22.677 ns |    28.679 ns |  0.0057 |      40 B |
| ChunkCore  | 1000             |   2,167.02 ns |    42.303 ns |    60.670 ns |  0.7820 |    4928 B |
| Chunk      | 10000            |  20,655.36 ns |   409.878 ns |   363.346 ns |  1.8005 |   11304 B |
| ChunkLight | 10000            |  11,824.50 ns |   228.439 ns |   271.941 ns |       - |      40 B |
| ChunkCore  | 10000            |  20,823.83 ns |   413.090 ns |   678.719 ns |  7.2021 |   45248 B |
| Chunk      | 100000           | 208,248.31 ns | 3,845.879 ns | 4,578.246 ns | 17.8223 |  112104 B |
| ChunkLight | 100000           | 119,780.17 ns | 2,380.495 ns | 2,444.593 ns |       - |      40 B |
| ChunkCore  | 100000           | 210,479.45 ns | 4,165.415 ns | 6,485.049 ns | 71.2891 |  448448 B |
