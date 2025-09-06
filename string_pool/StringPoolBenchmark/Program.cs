using BenchmarkDotNet.Running;
using StringPoolBenchmark;

var config = new BenchmarkConfig();
BenchmarkRunner.Run<StringPoolBenchmarks>(config);