using JetBrains.Annotations;
using StringPoolBenchmark.StringPools;

namespace StringPoolTests;

[UsedImplicitly]
public class StripedShardedTests : PoolTestsBase<StringPoolStripedSharded>
{
    protected override StringPoolStripedSharded CreatePool()
    {
        return new StringPoolStripedSharded(4, 2);
    }
}