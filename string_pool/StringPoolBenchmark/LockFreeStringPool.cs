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

            // Ensure only one ID is created for a string across threads and
            // that both dictionaries remain in sync.
            var id = state.StringToId.GetOrAdd(value, static (v, poolState) =>
            {
                var newId = Interlocked.Increment(ref poolState.NextId);
                poolState.IdToString[newId] = v;
                return newId;
            }, state);

            // State may change due to Clear().  If so, retry using the new state
            if (state != _state) continue;

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
        _state = new PoolState();
    }

    private class PoolState
    {
        public readonly ConcurrentDictionary<int, string> IdToString = new();
        public readonly ConcurrentDictionary<string, int> StringToId = new();
        public int NextId;
    }
}
