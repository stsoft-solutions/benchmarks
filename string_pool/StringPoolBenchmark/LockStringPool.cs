using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark;

public sealed class LockStringPool : IStringPool
{
    private readonly Dictionary<int, string> _idToString = new();
    private readonly Lock _lock = new();
    private readonly Dictionary<string, int> _stringToId = new();
    private int _nextId = 1;

    public int GetId(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        using (_lock.EnterScope())
        {
            if (_stringToId.TryGetValue(value, out var existingId))
                return existingId;

            var id = _nextId++;
            _stringToId[value] = id;
            _idToString[id] = value;
            return id;
        }
    }

    public bool TryGetString(int id, [MaybeNullWhen(false)] out string value)
    {
        using (_lock.EnterScope())
        {
            return _idToString.TryGetValue(id, out value);
        }
    }

    public void Clear()
    {
        using (_lock.EnterScope())
        {
            _stringToId.Clear();
            _idToString.Clear();
            _nextId = 1;
        }
    }
}