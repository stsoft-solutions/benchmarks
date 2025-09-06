using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark.StringPools;

public class LockFreeStringIdMap : IStringPool
{
    private sealed class Entry
    {
        public string? Key;
        public int Id;
    }

    private Entry[] _entries = [];
    private string[] _reverseMap = [];
    private int _nextId = 1;
    private int _threshold;

    public LockFreeStringIdMap(int initialCapacity)
    {
        InitTables(initialCapacity);
    }

    public LockFreeStringIdMap() : this(1 << 14)
    {
    }

    private static int NextPowerOfTwo(int value)
    {
        if (value < 2) return 2;
        // Round up to next power of two
        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        value++;
        return value;
    }

    private void InitTables(int capacity)
    {
        var cap = NextPowerOfTwo(capacity);

        _entries = new Entry[cap];
        for (var i = 0; i < cap; i++)
        {
            _entries[i] = new Entry();
        }

        _reverseMap = new string[cap];
        _threshold = (int)(cap * 0.75);
    }

    private int TryInsert(string key, out bool inserted)
    {
        var entries = _entries;
        var cap = entries.Length;
        var mask = cap - 1;
        var hash = (key.GetHashCode() & 0x7FFFFFFF);

        for (var i = 0; i < cap; i++)
        {
            var index = (hash + i) & mask;
            var entry = entries[index];

            var existing = Volatile.Read(ref entry.Key);
            if (existing == null)
            {
                if (Interlocked.CompareExchange(ref entry.Key, key, null) == null)
                {
                    var newId = Interlocked.Increment(ref _nextId) - 1;
                    entry.Id = newId;

                    EnsureReverseCapacity(newId);
                    _reverseMap[newId] = key;

                    inserted = true;
                    return newId;
                }

                existing = Volatile.Read(ref entry.Key);
            }

            if (!ReferenceEquals(existing, key) && !string.Equals(existing, key, StringComparison.Ordinal)) continue;
            
            inserted = true;
            return entry.Id;
        }

        inserted = false;
        return -1;
    }

    private bool TryGetId(string key, out int id)
    {
        var entries = _entries;
        var cap = entries.Length;
        var mask = cap - 1;
        var hash = (key.GetHashCode() & 0x7FFFFFFF);

        for (var i = 0; i < cap; i++)
        {
            var index = (hash + i) & mask;
            var entry = entries[index];

            var existing = Volatile.Read(ref entry.Key);
            if (existing == null)
                break;

            if (!ReferenceEquals(existing, key) && !string.Equals(existing, key, StringComparison.Ordinal)) continue;
            
            id = entry.Id;
            return true;
        }

        id = -1;
        return false;
    }

    private void EnsureReverseCapacity(long id)
    {
        while (id >= _reverseMap.Length)
        {
            var newMap = new string[_reverseMap.Length * 2];
            Array.Copy(_reverseMap, newMap, _reverseMap.Length);
            Interlocked.CompareExchange(ref _reverseMap, newMap, _reverseMap);
        }
    }

    private void Expand()
    {
        while (true)
        {
            var current = _entries;
            if (_nextId < _threshold)
                return;

            var newCap = current.Length * 2;
            var newEntries = new Entry[newCap];
            for (var i = 0; i < newCap; i++)
            {
                newEntries[i] = new Entry();
            }

            foreach (var entry in current)
            {
                if (entry.Key == null)
                    continue;

                var hash = (entry.Key.GetHashCode() & 0x7FFFFFFF);
                var mask = newCap - 1;
                for (var i = 0; i < newCap; i++)
                {
                    var index = (hash + i) & mask;
                    var newEntry = newEntries[index];

                    if (newEntry.Key == null)
                    {
                        newEntry.Key = entry.Key;
                        newEntry.Id = entry.Id;
                        break;
                    }
                }
            }

            if (Interlocked.CompareExchange(ref _entries, newEntries, current) == current)
            {
                Volatile.Write(ref _threshold, (int)(newCap * 0.75));
                return;
            }

            // Another thread expanded first; recompute a threshold based on current capacity and retry
            var capNow = Volatile.Read(ref _entries).Length;
            Volatile.Write(ref _threshold, (int)(capNow * 0.75));
        }
    }

    public int GetId(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (TryGetId(value, out var existingId)) return existingId;

        while (true)
        {
            var id = TryInsert(value, out var inserted);
            if (inserted)
                return id;

            Expand();
        }
    }

    public bool TryGetString(int id, [MaybeNullWhen(false)] out string value)
    {
        if (id >= 1 && id < _reverseMap.Length)
        {
            var v = _reverseMap[id];
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public void Clear()
    {
        InitTables(_entries.Length);
        _nextId = 1;
    }
}