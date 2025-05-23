using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

namespace StringPoolBenchmark;

[MemoryDiagnoser]
public class StringPoolBenchmarks
{
    private LockStringPool _lockPool = null!;
    private ReadWriteStringPool _rwLockPool = null!;
    private LockFreeStringPool _lockFreePool = null!;

    private string[] _testStrings = null!;
    [Params(1000, 50_000, 100_000)] public int DataSize { get; [UsedImplicitly] set; }

    [GlobalSetup]
    public void Setup()
    {
        _testStrings = Enumerable.Range(0, DataSize).Select(i => $"str{i}").ToArray();
        _lockPool = new LockStringPool();
        _rwLockPool = new ReadWriteStringPool();
        _lockFreePool = new LockFreeStringPool();
    }

    private void SingleThreadTest(IStringPool pool)
    {
        foreach (var s in _testStrings)
        {
            pool.GetId(s);
        }

        foreach (var s in _testStrings)
        {
            pool.TryGetString(_lockPool.GetId(s), out _);
        }
    }

    private void MultiThreadTest(IStringPool pool)
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
    public void Lock_SingleThread()
    {
        SingleThreadTest(_lockPool);
    }

    [Benchmark]
    public void RwLock_SingleThread()
    {
        SingleThreadTest(_rwLockPool);
    }

    [Benchmark]
    public void LockFree_SingleThread()
    {
        SingleThreadTest(_lockFreePool);
    }

    [Benchmark]
    public void Lock_MultiThread()
    {
        MultiThreadTest(_lockPool);
    }

    [Benchmark]
    public void RwLock_MultiThread()
    {
        MultiThreadTest(_rwLockPool);
    }

    [Benchmark]
    public void LockFree_MultiThread()
    {
        MultiThreadTest(_lockFreePool);
    }
}