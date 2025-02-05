using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable LoopCanBeConvertedToQuery

namespace ChunkEnumerableBenchmark;

[MemoryDiagnoser]
public class Benchmark
{
    private const int ChunkSize = 50;

    private IEnumerable<int> _numbers = null!;

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable once MemberCanBePrivate.Global
    [Params(10, 100, 500, 1_000, 10_000, 100_000)]
    public int NumberOfElements { get; set; } = 0;

    [GlobalSetup]
    public void Setup()
    {
        _numbers = Enumerable.Range(1, NumberOfElements);
    }

    [Benchmark]
    public long Chunk()
    {
        var result = 0L;
        foreach (var chunk in _numbers.ChunkEnumerable(ChunkSize))
        foreach (var i in chunk)
            result += i;

        return result;
    }

    [Benchmark]
    public long ChunkLight()
    {
        var result = 0L;
        foreach (var chunk in _numbers.ChunkEnumerableLight(ChunkSize))
        foreach (var i in chunk)
            result += i;

        return result;
    }

    [Benchmark]
    public long ChunkCore()
    {
        var result = 0L;
        foreach (var arrayChunk in _numbers.Chunk(ChunkSize))
        foreach (var i in arrayChunk)
            result += i;

        return result;
    }
}