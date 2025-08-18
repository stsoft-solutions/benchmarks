# String Pool Benchmark Suite

A .NET 9 solution that compares multiple implementations of a string-to-id/id-to-string pool. It includes:
- High-quality microbenchmarks built with BenchmarkDotNet
- Memory diagnostics (Allocated, Gen0/1/2)
- Exported results in Markdown, HTML, CSV (including per-measurement), and JSON
- A comprehensive unit test suite (56 tests)

This repository is intended to help evaluate trade-offs between lock-based, lock-free, striped/sharded, and concurrent-dictionary based designs for string interning/pooling scenarios.

## Projects

- StringPoolBenchmark
  - Runs benchmarks for several IStringPool implementations.
  - Uses a custom BenchmarkConfig that:
    - Writes artifacts to StringPoolBenchmark/BenchmarkResults
    - Exports results to .md, .html, .csv, .csv (measurements), and .json
    - Enables MemoryDiagnoser for memory columns in console and exports
    - Adds default column providers for a readable result table
  - Benchmarks included:
    - Add-and-Get (1K items): StringPoolAddAndGet1K
    - Add-and-Get (10K items): StringPoolAddAndGet10K
    - Concurrent (1K/10K, 1/4/16 threads): StringPoolConcurrentBenchmarks
- StringPoolTests
  - Validates pool behavior and edge cases. Currently 56 tests, all passing.

## Implementations under test

All implement the IStringPool interface: GetId(string), TryGetString(int, out string), Clear().

- StringPoolDictionaryLock: Dictionary + single lock
- StringPoolDictionaryReadwriteLock: Dictionary + ReaderWriterLockSlim
- StringPoolState: Lock-free-ish approach with an immutable-ish state swap for Clear()
- LockFreeStringIdMap: Open-addressing array with lock-free style operations and a reverse array
- StringPoolStripedSharded: Shards string->id and id->string maps to reduce contention

## Requirements

- .NET SDK 9.0+
- Recommended: Run in Release configuration for meaningful benchmark results

## Quick start

1) Restore & build
- From repository root:
  - dotnet build -c Release

2) Run unit tests
- From repository root:
  - dotnet test -c Release
- Expected: 56 tests pass

3) Run benchmarks (IDE)
- Set startup project to StringPoolBenchmark
- Configuration: Release
- Run. Artifacts are written to StringPoolBenchmark/BenchmarkResults

4) Run benchmarks (command line)
- From the project directory:
  - cd StringPoolBenchmark
  - dotnet run -c Release
- Or run the built executable directly:
  - StringPoolBenchmark\bin\Release\net9.0\StringPoolBenchmark.exe

## Where to find results

- All outputs are placed under:
  - StringPoolBenchmark/BenchmarkResults
- For each benchmark class/parameter set, you will see files like:
  - [BenchmarkName]-report.md (readable summary)
  - [BenchmarkName]-report.html
  - [BenchmarkName]-report.csv (summary)
  - [BenchmarkName]-measurements.csv (per-measurement data)
  - [BenchmarkName]-report.json

## Reading the results

- Console and exported tables include:
  - Method, Mean, Error, StdDev, and memory columns (Allocated, Gen0/1/2)
- Add-and-Get suites (1K/10K) use:
  - [Benchmark(OperationsPerInvoke = DataSize)] so you can reason about cost per item
  - IterationSetup clearing all pools before each iteration to avoid cross-contamination and ensure fairness
- Concurrent suite uses:
  - Parallel.ForEach with MaxDegreeOfParallelism = ThreadCount to control concurrency
  - Internal checks avoid exceptions in the hot path

### Why IterationSetup?

String pools are stateful; without clearing between iterations, later iterations would benefit from warmed caches or existing entries. IterationSetup calls Clear() on each pool before every iteration to guarantee each iteration performs the same amount of work, improving result comparability.

### Why OperationsPerInvoke?

BenchmarkDotNet sometimes reports "1 op" for each invocation. By setting OperationsPerInvoke = DataSize (1K or 10K), the framework normalizes mean time per logical operation, making comparisons clearer across implementations.

## Benchmark matrix

- Add-and-Get:
  - 1,000 and 10,000 unique strings
  - Measures the cost of inserting all strings and then retrieving by id
- Concurrent:
  - DataSize: 1,000 and 10,000
  - ThreadCount: 1, 4, 16
  - Measures multi-threaded insert followed by multi-threaded retrieval

## Troubleshooting

- Empty table or "No column providers defined" warning:
  - The benchmark project includes a ManualConfig with AddColumnProvider(DefaultColumnProviders.Instance). Ensure Program.cs uses the provided BenchmarkConfig when running via BenchmarkRunner.
- No memory columns:
  - MemoryDiagnoser is enabled by default in BenchmarkConfig. If you customize config, ensure AddDiagnoser(MemoryDiagnoser.Default) is present.
- Running in Debug:
  - Always run benchmarks in Release; Debug artifacts produce misleading results.

## Repository structure

- string-pool.slnx – Solution
- StringPoolBenchmark – BenchmarkDotNet project
  - Program.cs – Starts benchmarks using BenchmarkConfig
  - BenchmarkConfig.cs – Output and diagnoser configuration
  - StringPoolAddAndGet1K.cs – Fixed-size Add/Get (1K)
  - StringPoolAddAndGet10K.cs – Fixed-size Add/Get (10K)
  - StringPoolConcurrentBenchmarks.cs – Concurrency benchmarks
  - StringPools/* – Implementations under test
- StringPoolTests – Unit tests

## License

No explicit license file was found. If you intend to use or distribute this repository, please add a LICENSE file that fits your needs.

## Last updated

- 2025-08-18
