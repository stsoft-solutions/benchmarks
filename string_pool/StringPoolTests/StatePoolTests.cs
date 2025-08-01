using JetBrains.Annotations;
using StringPoolBenchmark;

namespace StringPoolTests;

[UsedImplicitly]
public class StatePoolTests : PoolTestsBase<StringPoolState>
{
    protected override StringPoolState CreatePool()
    {
        return new StringPoolState(1);
    }
}