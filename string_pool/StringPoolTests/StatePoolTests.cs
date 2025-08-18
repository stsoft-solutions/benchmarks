using JetBrains.Annotations;
using StringPoolBenchmark;
using StringPoolBenchmark.StringPools;

namespace StringPoolTests;

[UsedImplicitly]
public class StatePoolTests : PoolTestsBase<StringPoolState>
{
    protected override StringPoolState CreatePool()
    {
        return new StringPoolState(1);
    }
}