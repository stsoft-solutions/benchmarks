using JetBrains.Annotations;
using StringPoolBenchmark;

namespace StringPoolTests;

[UsedImplicitly]
public class LockFreeStringIdMapTests : PoolTestsBase<LockFreeStringIdMap>
{
    protected override LockFreeStringIdMap CreatePool()
    {
        return new LockFreeStringIdMap();
    }
}