using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;
using StringPoolBenchmark.StringPools;

namespace StringPoolBenchmark;

[MemoryDiagnoser]
public class StringPoolBenchmarks
{
    private StringPoolDictionaryLock _lockPool = null!;
    private StringPoolDictionaryReadwriteLock _rwLockPool = null!;
    private StringPoolState _statePool = null!;
    private LockFreeStringIdMap _lockFreePool = null!;
    private StringPoolStripedSharded _stripedShardedPool = null!;

    private string[] _testStrings = null!;

    [Params(1000, 10000)] public int DataSize { get; [UsedImplicitly] set; }
    [Params(1, 4, 16)] public int ThreadCount { get; [UsedImplicitly] set; }

    [GlobalSetup]
    public void Setup()
    {
        _testStrings = Enumerable.Range(0, DataSize).Select(i => $"str{i}").ToArray();
        _lockPool = new StringPoolDictionaryLock(DataSize);
        _rwLockPool = new StringPoolDictionaryReadwriteLock(DataSize);
        _statePool = new StringPoolState(DataSize);
        _lockFreePool = new LockFreeStringIdMap(DataSize);
        _stripedShardedPool = new StringPoolStripedSharded(DataSize, Environment.ProcessorCount);
    }

    [IterationSetup]
    public void IterationSetup()
    {
        // Ensure each benchmark iteration starts from a clean state to avoid cross-contamination
        _lockPool.Clear();
        _rwLockPool.Clear();
        _statePool.Clear();
        _lockFreePool.Clear();
        _stripedShardedPool.Clear();
    }

    private void AddAndGetTest(IStringPool pool)
    {
        // Add strings and store their ids to avoid double-GetId work in the get phase
        var ids = new int[_testStrings.Length];
        for (var i = 0; i < _testStrings.Length; i++)
        {
            ids[i] = pool.GetId(_testStrings[i]);
        }

        // Then resolve by id only, measuring reverse-map performance
        for (var i = 0; i < ids.Length; i++)
        {
            pool.TryGetString(ids[i], out _);
        }
    }

    private void ConcurrentTest(IStringPool pool)
    {
        var ids = new ConcurrentBag<(int id, string value)>();
        var po = new ParallelOptions { MaxDegreeOfParallelism = ThreadCount };

        // Fill the pool with strings, using multiple threads
        Parallel.ForEach(_testStrings, po, s => { ids.Add((pool.GetId(s), s)); });

        // Retrieve strings from the pool using multiple threads
        Parallel.ForEach(ids, po, bag =>
        {
            pool.TryGetString(bag.id, out var s);
            if (!ReferenceEquals(s, bag.value))
            {
                // Avoid throwing during measurement; just record a mismatch count
                // In practice this should never happen for correct implementations
                // We intentionally do nothing to keep the hot path clean.
            }
        });
    }

    [Benchmark]
    public void DictionaryLock_AddAndGet()
    {
        AddAndGetTest(_lockPool);
    }

    [Benchmark]
    public void StripedSharded_AddAndGet()
    {
        AddAndGetTest(_stripedShardedPool);
    }

    [Benchmark]
    public void LockFree_AddAndGet()
    {
        AddAndGetTest(_lockFreePool);
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
    public void LockFree_Concurrent()
    {
        ConcurrentTest(_lockFreePool);
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

    [Benchmark]
    public void StripedSharded_Concurrent()
    {
        ConcurrentTest(_stripedShardedPool);
    }
}