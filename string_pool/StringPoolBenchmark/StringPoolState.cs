using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark;

public sealed class StringPoolState : IStringPool
{
    private volatile PoolState _state = new();


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
        // Atomically swap in a new state
        _state = new PoolState();
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
        public readonly ConcurrentDictionary<int, string> IdToString = new();
        public readonly ConcurrentDictionary<string, int> StringToId = new();
        public int NextId;
    }
}