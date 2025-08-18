using BenchmarkDotNet.Running;
using StringPoolBenchmark;

var config = new BenchmarkConfig();
BenchmarkRunner.Run<StringPoolAddAndGet1K>(config);
BenchmarkRunner.Run<StringPoolAddAndGet10K>(config);
BenchmarkRunner.Run<StringPoolConcurrentBenchmarks>(config);