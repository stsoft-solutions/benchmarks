using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark;

public sealed class StringPoolDictionaryLock : IStringPool
{
    private readonly Dictionary<int, string> _idToString;
    private readonly Lock _lock = new();
    private readonly Dictionary<string, int> _stringToId;
    private int _nextId = 1;

    public StringPoolDictionaryLock(int initialCapacity)
    {
        _idToString = new Dictionary<int, string>(initialCapacity);
        _stringToId = new Dictionary<string, int>(initialCapacity);
    }

    public int GetId(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        lock (_lock)
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
        lock (_lock)
        {
            return _idToString.TryGetValue(id, out value);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _stringToId.Clear();
            _idToString.Clear();
            _nextId = 1;
        }
    }
}