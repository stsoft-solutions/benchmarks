using JetBrains.Annotations;
using StringPoolBenchmark.StringPools;

namespace StringPoolTests;

[UsedImplicitly]
public class DictionaryReadwriteLockTests : PoolTestsBase<StringPoolDictionaryReadwriteLock>
{
    protected override StringPoolDictionaryReadwriteLock CreatePool()
    {
        return new StringPoolDictionaryReadwriteLock(4);
    }
}