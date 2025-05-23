using StringPoolBenchmark;

namespace StringPoolTests;

public class StatePoolTests
{
    [Fact]
    public void GetId_ShouldReturnSameIdForSameString()
    {
        var pool = new StringPoolState();
        var id1 = pool.GetId("test");
        var id2 = pool.GetId("test");
        Assert.Equal(id1, id2);
    }

    [Fact]
    public void GetId_ShouldReturnDifferentIdsForDifferentStrings()
    {
        var pool = new StringPoolState();
        var id1 = pool.GetId("test1");
        var id2 = pool.GetId("test2");
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void TryGetString_ShouldReturnTrueAndCorrectStringForValidId()
    {
        var pool = new StringPoolState();
        var id = pool.GetId("test");
        var result = pool.TryGetString(id, out var value);
        Assert.True(result);
        Assert.Equal("test", value);
    }

    [Fact]
    public void TryGetString_ShouldReturnFalseForInvalidId()
    {
        var pool = new StringPoolState();
        var result = pool.TryGetString(999, out var value);
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void Clear_ShouldRemoveAllEntries()
    {
        var pool = new StringPoolState();
        pool.GetId("test");
        pool.Clear();
        var result = pool.TryGetString(1, out var value);
        Assert.False(result);
        Assert.Null(value);
    }

    [Fact]
    public void GetId_ShouldThrowArgumentNullExceptionForNullString()
    {
        var pool = new StringPoolState();
        Assert.Throws<ArgumentNullException>(() => pool.GetId(null!));
    }
}