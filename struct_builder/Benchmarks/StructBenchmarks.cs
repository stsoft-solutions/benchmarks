using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

public class ReadBenchmarks : BenchmarksBase
{
    [Benchmark(Baseline = true)]
    public long ReadDefault()
    {
        long sum = 0;
        foreach (var key in KeysToLookup)
        {
            if (DictDefault.TryGetValue(key, out var value)) sum += value.IntValue + (long)value.DoubleValue + value.LongValue;
        }

        return sum;
    }

    [Benchmark]
    public long ReadP0()
    {
        long sum = 0;
        foreach (var key in KeysToLookup)
        {
            if (DictP0.TryGetValue(key, out var value)) sum += value.IntValue + (long)value.DoubleValue + value.LongValue;
        }

        return sum;
    }

    [Benchmark]
    public long ReadP1()
    {
        long sum = 0;
        foreach (var key in KeysToLookup)
        {
            if (DictP1.TryGetValue(key, out var value)) sum += value.IntValue + (long)value.DoubleValue + value.LongValue;
        }

        return sum;
    }

    [Benchmark]
    public long ReadDefaultClass()
    {
        long sum = 0;
        foreach (var key in KeysToLookup)
        {
            if (DictDefaultClass.TryGetValue(key, out var value)) sum += value.IntValue + (long)value.DoubleValue + value.LongValue;
        }

        return sum;
    }

    [Benchmark]
    public long ReadDefaultRecord()
    {
        long sum = 0;
        foreach (var key in KeysToLookup)
        {
            if (DictDefaultRecord.TryGetValue(key, out var value)) sum += value.IntValue + (long)value.DoubleValue + value.LongValue;
        }

        return sum;
    }
}