using JetBrains.Annotations;
using StringPoolBenchmark;

namespace StringPoolTests;

[UsedImplicitly]
public class StringPoolDictionaryLockTests : PoolTestsBase<StringPoolDictionaryLock>
{
    protected override StringPoolDictionaryLock CreatePool()
    {
        return new StringPoolDictionaryLock(1);
    }
}