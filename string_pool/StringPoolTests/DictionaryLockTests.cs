using JetBrains.Annotations;
using StringPoolBenchmark.StringPools;

namespace StringPoolTests;

[UsedImplicitly]
public class DictionaryLockTests : PoolTestsBase<StringPoolDictionaryLock>
{
    protected override StringPoolDictionaryLock CreatePool()
    {
        return new StringPoolDictionaryLock(4);
    }
}