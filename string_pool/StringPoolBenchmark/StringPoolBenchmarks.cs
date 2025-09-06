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

    [Params(10_000, 500_000, 2_000_000)] public int DataSize { get; [UsedImplicitly] set; }
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
        // Use a simple array to avoid ConcurrentBag overhead and avoid retaining string references
        var ids = new int[_testStrings.Length];
        var i = 0;
        foreach (var s in _testStrings)
        {
            ids[i++] = pool.GetId(s);
        }

        // Validation pass: ensure ids are retrievable without comparing to original strings
        foreach (var id in ids)
        {
            pool.TryGetString(id, out _);
        }
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ConcurrentTest(IStringPool pool)
    {
        var po = new ParallelOptions { MaxDegreeOfParallelism = ThreadCount };

        // Aggregate ids per thread to minimize contention and GC
        var partitions = Partitioner.Create(0, _testStrings.Length);
        var allIds = new int[_testStrings.Length];

        Parallel.ForEach(partitions, po, () => 0, (range, state, local) =>
        {
            for (int i = range.Item1; i < range.Item2; i++)
            {
                allIds[i] = pool.GetId(_testStrings[i]);
            }
            return local;
        }, _ => { });

        // Retrieve strings from the pool using multiple threads
        Parallel.For(0, allIds.Length, po, i =>
        {
            pool.TryGetString(allIds[i], out _);
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