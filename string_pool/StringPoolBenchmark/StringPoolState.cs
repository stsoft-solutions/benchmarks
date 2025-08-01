using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark;

public sealed class StringPoolState : IStringPool
{
    private readonly int _initialCapacity;
    private volatile PoolState _state;

    public StringPoolState(int initialCapacity)
    {
        _initialCapacity = initialCapacity;
        _state = new PoolState(_initialCapacity);
    }

    public int GetId(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        while (true)
        {
            var state = _state;

            // Try to get or add the string atomically
            int newId;
            var added = false;
            var id = state.StringToId.GetOrAdd(value, _ =>
            {
                newId = Interlocked.Increment(ref state.NextId);
                added = true;
                return newId;
            });

            if (added)
                // Only the thread that added the string should add to IdToString
                state.IdToString.TryAdd(id, value);

            return id;
        }
    }

    public bool TryGetString(int id, [MaybeNullWhen(false)] out string value)
    {
        var state = _state;
        return state.IdToString.TryGetValue(id, out value);
    }

    public void Clear()
    {
        // Atomic swap in a new state
        _state = new PoolState(_initialCapacity);
    }

    public int GetId1(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        while (true)
        {
            var state = _state;

            if (state.StringToId.TryGetValue(value, out var existingId))
                return existingId;

            var newId = Interlocked.Increment(ref state.NextId);

            // If another thread added it, retry
            if (!state.StringToId.TryAdd(value, newId))
                continue;

            state.IdToString.TryAdd(newId, value);

            return newId;
        }
    }

    private class PoolState
    {
        public PoolState(int initialCapacity)
        {
            IdToString = new ConcurrentDictionary<int, string>(-1, initialCapacity);
            StringToId = new ConcurrentDictionary<string, int>(-1, initialCapacity);
        }
        
        public readonly ConcurrentDictionary<int, string> IdToString;
        public readonly ConcurrentDictionary<string, int> StringToId;
        
        public int NextId;
    }
}