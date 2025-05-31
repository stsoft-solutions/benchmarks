using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class EnumerateStructBenchmarks : BenchmarksBase
{
    [Benchmark(Baseline = true)]
    public long EnumerateDefault()
    {
        long sum = 0;
        foreach (var item in DictDefault.Values)
        {
            sum += item.IntValue + (long)item.DoubleValue + item.LongValue;
        }

        return sum;
    }

    [Benchmark]
    public long EnumerateP0()
    {
        long sum = 0;
        foreach (var item in DictP0.Values)
        {
            sum += item.IntValue + (long)item.DoubleValue + item.LongValue;
        }

        return sum;
    }

    [Benchmark]
    public long EnumerateP1()
    {
        long sum = 0;
        foreach (var item in DictP1.Values)
        {
            sum += item.IntValue + (long)item.DoubleValue + item.LongValue;
        }

        return sum;
    }

    [Benchmark]
    public long EnumerateDefaultClass()
    {
        long sum = 0;
        foreach (var item in DictDefaultClass.Values)
        {
            sum += item.IntValue + (long)item.DoubleValue + item.LongValue;
        }

        return sum;
    }

    [Benchmark]
    public long EnumerateDefaultRecord()
    {
        long sum = 0;
        foreach (var item in DictDefaultRecord.Values)
        {
            sum += item.IntValue + (long)item.DoubleValue + item.LongValue;
        }

        return sum;
    }
}