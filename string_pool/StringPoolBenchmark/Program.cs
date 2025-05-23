using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using JetBrains.Annotations;
using StringPoolBenchmark;

BenchmarkRunner.Run<StringPoolBenchmarks>();

[MemoryDiagnoser]
public class StringPoolBenchmarks
{
    [Params(1000, 10000)] public int DataSize { get; [UsedImplicitly] set; }

    [Params(1, 8)] public int ThreadCount { get; [UsedImplicitly] set; }

    private string[] _testStrings = null!;
    private StringPoolDictionaryLock _lockPool = null!;
    private StringPoolDictionaryReadwriteLock _rwLockPool = null!;

    [GlobalSetup]
    public void Setup()
    {
        _testStrings = Enumerable.Range(0, DataSize).Select(i => $"str{i}").ToArray();
        _lockPool = new StringPoolDictionaryLock();
        _rwLockPool = new StringPoolDictionaryReadwriteLock();
    }

    private void AddAndGetTest(IStringPool pool)
    {
        foreach (var s in _testStrings)
            pool.GetId(s);

        foreach (var s in _testStrings)
            pool.TryGetString(_lockPool.GetId(s), out _);
    }

    private void ConcurrentTest(IStringPool pool)
    {
        var ids = new ConcurrentBag<int>();
        
        // Fill the pool with strings, using multiple threads
        Parallel.ForEach(_testStrings, new ParallelOptions { MaxDegreeOfParallelism = ThreadCount }, s =>
        {
            ids.Add(pool.GetId(s));
        });
        
        // Retrieve strings from the pool using multiple threads
        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ThreadCount }, id =>
        {
            pool.TryGetString(id, out _);
        });
    }

    [Benchmark]
    public void DictionaryLock_AddAndGet()
    {
        AddAndGetTest(_lockPool);
    }

    [Benchmark]
    public void DictionaryReadwriteLock_AddAndGet()
    {
        AddAndGetTest(_rwLockPool);
    }

    [Benchmark]
    public void DictionaryLock_Concurrent()
    {
        ConcurrentTest(_lockPool);
    }

    [Benchmark]
    public void DictionaryReadwriteLock_Concurrent()
    {
        ConcurrentTest(_rwLockPool);
    }
}