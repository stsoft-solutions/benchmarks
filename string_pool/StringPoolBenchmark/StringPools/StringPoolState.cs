using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark.StringPools;

public sealed class StringPoolState : IStringPool
{
    private readonly ConcurrentDictionary<int, string> _idToString;
    private readonly ConcurrentDictionary<string, int> _stringToId;
    private readonly object _lock = new(); // Changed from Lock to object for standard C# locking
    private int _nextId = 1;

    public StringPoolState(int initialCapacity)
    {
        _idToString = new ConcurrentDictionary<int, string>(Environment.ProcessorCount, initialCapacity);
        _stringToId = new ConcurrentDictionary<string, int>(Environment.ProcessorCount, initialCapacity);
    }

    public int GetId(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        // Fast path: lock-free read
        if (_stringToId.TryGetValue(value, out var existingId))
            return existingId;

        // Slow path: lock to ensure atomicity of ID generation and insertion
        lock (_lock)
        {
            // Double-check locking
            if (_stringToId.TryGetValue(value, out existingId))
                return existingId;

            var id = _nextId++;
            _stringToId.TryAdd(value, id);
            _idToString.TryAdd(id, value);
            return id;
        }
    }

    public bool TryGetString(int id, [MaybeNullWhen(false)] out string value)
    {
        return _idToString.TryGetValue(id, out value);
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