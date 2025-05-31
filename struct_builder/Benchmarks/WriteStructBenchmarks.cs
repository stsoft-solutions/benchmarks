using BenchmarkDotNet.Attributes;
using Benchmarks.Model;

// ReSharper disable CollectionNeverQueried.Local

namespace Benchmarks;

public class WriteBenchmarks : BenchmarksBase
{
    [Benchmark(Baseline = true)]
    public void WriteDefault()
    {
        var dict = new Dictionary<int, ConstructorStructDefaultLayout>(ItemCount);

        for (var i = 0; i < ItemCount; i++)
        {
            dict[i] = new ConstructorStructDefaultLayout(
                Rnd.Next(),
                Rnd.NextDouble() * 100,
                DateTime.Now,
                (char)Rnd.Next(65, 90),
                Rnd.NextInt64(),
                (short)Rnd.Next(short.MinValue, short.MaxValue),
                Rnd.Next(2) == 1,
                Rnd.NextDouble() * 100);
        }
    }

    [Benchmark]
    public void WriteP0()
    {
        var dict = new Dictionary<int, ConstructorStructP0>(ItemCount);

        for (var i = 0; i < ItemCount; i++)
        {
            dict[i] = new ConstructorStructP0(
                Rnd.Next(),
                Rnd.NextDouble() * 100,
                DateTime.Now,
                (char)Rnd.Next(65, 90),
                Rnd.NextInt64(),
                (short)Rnd.Next(short.MinValue, short.MaxValue),
                Rnd.Next(2) == 1,
                Rnd.NextDouble() * 100);
        }
    }

    [Benchmark]
    public void WriteP1()
    {
        var dict = new Dictionary<int, ConstructorStructP1>(ItemCount);

        for (var i = 0; i < ItemCount; i++)
        {
            dict[i] = new ConstructorStructP1(
                Rnd.Next(),
                Rnd.NextDouble() * 100,
                DateTime.Now,
                (char)Rnd.Next(65, 90),
                Rnd.NextInt64(),
                (short)Rnd.Next(short.MinValue, short.MaxValue),
                Rnd.Next(2) == 1,
                Rnd.NextDouble() * 100);
        }
    }

    [Benchmark]
    public void WriteClass()
    {
        var dict = new Dictionary<int, DefaultClass>(ItemCount);

        for (var i = 0; i < ItemCount; i++)
        {
            dict[i] = new DefaultClass(
                Rnd.Next(),
                Rnd.NextDouble() * 100,
                DateTime.Now,
                (char)Rnd.Next(65, 90),
                Rnd.NextInt64(),
                (short)Rnd.Next(short.MinValue, short.MaxValue),
                Rnd.Next(2) == 1,
                Rnd.NextDouble() * 100);
        }
    }

    [Benchmark]
    public void WriteRecord()
    {
        var dict = new Dictionary<int, DefaultRecord>(ItemCount);

        for (var i = 0; i < ItemCount; i++)
        {
            dict[i] = new DefaultRecord(
                Rnd.Next(),
                Rnd.NextDouble() * 100,
                DateTime.Now,
                (char)Rnd.Next(65, 90),
                Rnd.NextInt64(),
                (short)Rnd.Next(short.MinValue, short.MaxValue),
                Rnd.Next(2) == 1,
                Rnd.NextDouble() * 100);
        }
    }
}