using BenchmarkDotNet.Running;
using StringPoolBenchmark;

BenchmarkRunner.Run<StringPoolAddAndGetBenchmarks>();
BenchmarkRunner.Run<StringPoolConcurrentBenchmarks>();