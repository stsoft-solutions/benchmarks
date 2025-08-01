using JetBrains.Annotations;
using StringPoolBenchmark;

namespace StringPoolTests;

[UsedImplicitly]
public class StringPoolDictionaryReadWriteLockTests : PoolTestsBase<StringPoolDictionaryReadwriteLock>
{
    protected override StringPoolDictionaryReadwriteLock CreatePool()
    {
        return new StringPoolDictionaryReadwriteLock(1);
    }
}