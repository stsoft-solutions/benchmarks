using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;
using StringPoolBenchmark.StringPools;

namespace StringPoolBenchmark;

[MemoryDiagnoser]
public class StringPoolAddAndGet500K
{
    private const int DataSize = 500_000;

    private StringPoolDictionaryLock _lockPool = null!;
    private StringPoolDictionaryReadwriteLock _rwLockPool = null!;
    private StringPoolState _statePool = null!;
    private LockFreeStringIdMap _lockFreePool = null!;
    private StringPoolStripedSharded _stripedShardedPool = null!;

    private string[] _testStrings = null!;

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

    private void AddAndGetTest(IStringPool pool)
    {
        var ids = new int[_testStrings.Length];
        for (var i = 0; i < _testStrings.Length; i++)
        {
            ids[i] = pool.GetId(_testStrings[i]);
        }

        for (var i = 0; i < ids.Length; i++)
        {
            pool.TryGetString(ids[i], out _);
        }
    }

    [Benchmark(OperationsPerInvoke = DataSize)]
    public void DictionaryLock_AddAndGet() => AddAndGetTest(_lockPool);

    [Benchmark(OperationsPerInvoke = DataSize)]
    public void StripedSharded_AddAndGet() => AddAndGetTest(_stripedShardedPool);

    [Benchmark(OperationsPerInvoke = DataSize)]
    public void LockFree_AddAndGet() => AddAndGetTest(_lockFreePool);

    [Benchmark(OperationsPerInvoke = DataSize)]
    public void StatePool_AddAndGet() => AddAndGetTest(_statePool);

    [Benchmark(OperationsPerInvoke = DataSize)]
    public void DictionaryReadwriteLock_AddAndGet() => AddAndGetTest(_rwLockPool);
}