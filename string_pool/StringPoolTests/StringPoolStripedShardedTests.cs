using JetBrains.Annotations;
using StringPoolBenchmark;
using StringPoolBenchmark.StringPools;

namespace StringPoolTests;

[UsedImplicitly]
public class StringPoolStripedShardedTests : PoolTestsBase<StringPoolStripedSharded>
{
    protected override StringPoolStripedSharded CreatePool()
    {
        // Use 1 shard to make the first assigned id equal to 1 per test expectations
        return new StringPoolStripedSharded(initialCapacity: 1, shardCount: 1);
    }
}
