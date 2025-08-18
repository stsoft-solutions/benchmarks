using JetBrains.Annotations;
using StringPoolBenchmark;
using StringPoolBenchmark.StringPools;

namespace StringPoolTests;

[UsedImplicitly]
public class StringPoolDictionaryLockTests : PoolTestsBase<StringPoolDictionaryLock>
{
    protected override StringPoolDictionaryLock CreatePool()
    {
        return new StringPoolDictionaryLock(1);
    }
}