using JetBrains.Annotations;
using StringPoolBenchmark.StringPools;

namespace StringPoolTests;

[UsedImplicitly]
public class LockFreeStringIdMapTests : PoolTestsBase<LockFreeStringIdMap>
{
    protected override LockFreeStringIdMap CreatePool()
    {
        return new LockFreeStringIdMap(4);
    }
}