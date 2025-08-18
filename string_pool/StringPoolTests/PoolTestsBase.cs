using StringPoolBenchmark;
using StringPoolBenchmark.StringPools;

namespace StringPoolTests;

public abstract class PoolTestsBase<TPool> where TPool : IStringPool
{
    protected abstract TPool CreatePool();
    
    [Fact]
    public void GetId_ShouldReturnSameIdForSameString()
    {
        var pool = CreatePool();
        var id1 = pool.GetId("test");
        var id2 = pool.GetId("test");
        Assert.Equal(id1, id2);
    }

    [Fact]
    public void GetId_ShouldReturnOneForFirstString()
    {
        var pool = CreatePool();
        var id1 = pool.GetId("test");
        Assert.Equal(1, id1);
    }

    [Fact]
    public void GetId_ShouldReturnDifferentIdsForDifferentStrings()
    {
        var pool = CreatePool();
        var id1 = pool.GetId("test1");
        var id2 = pool.GetId("test2");
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void TryGetString_ShouldReturnTrueAndCorrectStringForValidId()
    {
        var pool = CreatePool();
        var id = pool.GetId("test");
        var result = pool.TryGetString(id, out var value);
        Assert.True(result);
        Assert.Equal("test", value);
    }

    [Fact]
    public void TryGetString_ShouldReturnFalseForInvalidId()
    {
        var pool = CreatePool();
        var result = pool.TryGetString(999, out var value);
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void Clear_ShouldRemoveAllEntries()
    {
        var pool = CreatePool();
        pool.GetId("test");
        pool.Clear();
        var result = pool.TryGetString(1, out var value);
        Assert.False(result);
        Assert.Null(value);
    }


    [Fact]
    public void GetId_ShouldThrowArgumentNullExceptionForNullString()
    {
        var pool = new StringPoolState(1);
        Assert.Throws<ArgumentNullException>(() => pool.GetId(null!));
    }
}