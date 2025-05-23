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
        foreach (var s in _testStrings)
        {
            pool.GetId(s);
        }

        foreach (var s in _testStrings)
        {
            pool.TryGetString(pool.GetId(s), out _);
        }
    }

    private void MultiThreadTest(IStringPool pool)
    {
        // Fill the pool with strings, using multiple threads
        Parallel.ForEach(_testStrings,
            s => pool.GetId(s));

        // Retrieve strings from the pool using multiple threads
        Parallel.ForEach(_testStrings,
            s =>
            {
                pool.TryGetString(pool.GetId(s), out var poolString);

                if (!ReferenceEquals(s, poolString))
                    throw new Exception($"Expected {poolString}, but got {s}");
            });
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