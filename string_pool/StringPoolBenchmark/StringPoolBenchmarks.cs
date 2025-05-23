using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

namespace StringPoolBenchmark;

[MemoryDiagnoser]
public class StringPoolBenchmarks
{
    private StringPoolDictionaryLock _lockPool = null!;
    private StringPoolDictionaryReadwriteLock _rwLockPool = null!;
    private StringPoolState _statePool = null!;

    private string[] _testStrings = null!;
    [Params(1000, 10000)] public int DataSize { get; [UsedImplicitly] set; }

    [GlobalSetup]
    public void Setup()
    {
        _testStrings = Enumerable.Range(0, DataSize).Select(i => $"str{i}").ToArray();
        _lockPool = new StringPoolDictionaryLock();
        _rwLockPool = new StringPoolDictionaryReadwriteLock();
        _statePool = new StringPoolState();
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
        var ids = new ConcurrentBag<(int id, string value)>();

        // Fill the pool with strings, using multiple threads
        Parallel.ForEach(_testStrings, 
            s => { ids.Add((pool.GetId(s), s)); });

        // Retrieve strings from the pool using multiple threads
        Parallel.ForEach(ids, 
            bag =>
            {
                pool.TryGetString(bag.id, out var s);

                if (!ReferenceEquals(s, bag.value))
                    throw new Exception($"Expected {bag.value}, but got {s}");
            });
    }

    [Benchmark]
    public void DictionaryLock_AddAndGet()
    {
        AddAndGetTest(_lockPool);
    }

    [Benchmark]
    public void StatePool_AddAndGet()
    {
        AddAndGetTest(_statePool);
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
    public void StatePool_Concurrent()
    {
        ConcurrentTest(_statePool);
    }

    [Benchmark]
    public void DictionaryReadwriteLock_Concurrent()
    {
        ConcurrentTest(_rwLockPool);
    }
}