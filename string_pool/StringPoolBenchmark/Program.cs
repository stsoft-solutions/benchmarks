using BenchmarkDotNet.Running;
using StringPoolBenchmark;

var config = new BenchmarkConfig();
BenchmarkRunner.Run<StringPoolAddAndGet10K>(config);
BenchmarkRunner.Run<StringPoolAddAndGet500K>(config);
BenchmarkRunner.Run<StringPoolConcurrentBenchmarks>(config);