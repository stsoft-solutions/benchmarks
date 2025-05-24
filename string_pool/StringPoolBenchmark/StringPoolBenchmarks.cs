using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

namespace StringPoolBenchmark;

[MemoryDiagnoser]
public class StringPoolBenchmarks
{
    private string[] _testStrings = null!;
    [Params(1000, 50_000, 100_000)] public int DataSize { get; [UsedImplicitly] set; }

    [GlobalSetup]
    public void Setup()
    {
        _testStrings = Enumerable.Range(0, DataSize).Select(i => $"str{i}").ToArray();
    }

    private void SingleThreadTest(IStringPool pool)
    {
        var ids = new ConcurrentBag<int>();

        foreach (var s in _testStrings)
        {
            // Get the ID for each string and store it in a thread-safe collection
            ids.Add(pool.GetId(s));
        }

        foreach (var id in ids)
        {
            _ = pool.TryGetString(id, out _);
        }
    }

    private void MultiThreadTest(IStringPool pool)
    {
        var ids = new ConcurrentBag<int>();

        // Fill the pool with strings, using multiple threads
        Parallel.ForEach(_testStrings,
            s => ids.Add(pool.GetId(s)));

        // Retrieve strings from the pool using multiple threads
        Parallel.ForEach(ids,
            id => _ = pool.TryGetString(id, out var poolString));
    }

    [Benchmark]
    public void Lock_SingleThread()
    {
        var lockPool = new LockStringPool();
        SingleThreadTest(lockPool);
    }

    [Benchmark]
    public void RwLock_SingleThread()
    {
        var rwLockPool = new ReadWriteStringPool();
        SingleThreadTest(rwLockPool);
    }

    [Benchmark]
    public void LockFree_SingleThread()
    {
        var lockFreePool = new LockFreeStringPool();
        SingleThreadTest(lockFreePool);
    }

    [Benchmark]
    public void Lock_MultiThread()
    {
        var lockPool = new LockStringPool();
        MultiThreadTest(lockPool);
    }

    [Benchmark]
    public void RwLock_MultiThread()
    {
        var rwLockPool = new ReadWriteStringPool();
        MultiThreadTest(rwLockPool);
    }

    [Benchmark]
    public void LockFree_MultiThread()
    {
        var lockFreePool = new LockFreeStringPool();
        MultiThreadTest(lockFreePool);
    }
}