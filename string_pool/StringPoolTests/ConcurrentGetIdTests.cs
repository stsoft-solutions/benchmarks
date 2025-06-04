using JetBrains.Annotations;
using StringPoolBenchmark;

namespace StringPoolTests;

[UsedImplicitly]
public class ConcurrentGetIdTests
{
    [Fact]
    public void GetId_ShouldReturnSameId_WhenCalledFromMultipleThreads()
    {
        const int threadCount = 8;
        const string value = "concurrent";
        var pool = new LockFreeStringPool();
        var ids = new int[threadCount];
        var threads = new Thread[threadCount];

        for (var i = 0; i < threadCount; i++)
        {
            var index = i;
            threads[i] = new Thread(() => ids[index] = pool.GetId(value));
            threads[i].Start();
        }

        foreach (var t in threads)
            t.Join();

        for (var i = 1; i < threadCount; i++)
            Assert.Equal(ids[0], ids[i]);

        Assert.True(pool.TryGetString(ids[0], out var retrieved));
        Assert.Equal(value, retrieved);
    }
}
