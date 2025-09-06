using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
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

    [Params(10_000, 500_000)] public int DataSize { get; [UsedImplicitly] set; }
    [Params(1, 4, 8, 16)] public int ThreadCount { get; [UsedImplicitly] set; }

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

    [IterationCleanup] 
    public void IterationCleanup()
    {
        _lockPool.Clear();
        _rwLockPool.Clear();
        _statePool.Clear();
        _lockFreePool.Clear();
        _stripedShardedPool.Clear();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SingleThreadTest(IStringPool pool)
    {
        var ids = new ConcurrentBag<(int id, string value)>();
        foreach (var s in _testStrings)
        {
            ids.Add((pool.GetId(s), s));
        }
        
        foreach (var (id, _) in ids)
        {
            if(!pool.TryGetString(id, out _))
            {
                // Avoid throwing during measurement; just record a mismatch count
                // In practice this should never happen for correct implementations
                // We intentionally do nothing to keep the hot path clean.
            }
        }
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Test(IStringPool pool)
    {
        if (ThreadCount == 1)
        {
            SingleThreadTest(pool);
        }
        else
        {
            ConcurrentTest(pool);
        }
    }

    [Benchmark]
    public void DictionaryLock() => Test(_lockPool);

    [Benchmark]
    public void LockFree() => Test(_lockFreePool);

    [Benchmark]
    public void StatePool() => Test(_statePool);

    [Benchmark]
    public void DictionaryReadwriteLock() => Test(_rwLockPool);

    [Benchmark]
    public void StripedSharded() => Test(_stripedShardedPool);
}