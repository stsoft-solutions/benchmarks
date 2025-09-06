using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark;

public sealed class LockFreeStringPool : IStringPool
{
    private volatile PoolState _state = new();

    public int GetId(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        while (true)
        {
            var state = _state;

            // Try to get the ID for the string
            if (state.StringToId.TryGetValue(value, out var id)) return id;

            // Get a new ID
            var newId = Interlocked.Increment(ref state.NextId);

            // Try to add the new ID to the dictionary if it fails, retry
            if (!state.IdToString.TryAdd(newId, value)) continue;

            // Add the string to the dictionary
            state.StringToId[value] = newId;

            // Check if the state has changed since we started
            // If it has, we need to retry
            if (state != _state) continue;

            return newId;
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
        _state = new PoolState();
    }

    private class PoolState
    {
        public readonly ConcurrentDictionary<int, string> IdToString = new();
        public readonly ConcurrentDictionary<string, int> StringToId = new();
        public int NextId;
    }
}