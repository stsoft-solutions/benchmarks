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
    private const string DeletedMarker = "\uFFFF__DELETED__";

    public LockFreeStringIdMap(int initialCapacity)
    {
        InitTables(initialCapacity);
    }

    public LockFreeStringIdMap() : this(1 << 14)
    {
    }

    private void InitTables(int capacity)
    {
        _entries = new Entry[capacity];
        for (var i = 0; i < capacity; i++)
        {
            _entries[i] = new Entry();
        }

        _reverseMap = new string[capacity];
        _threshold = (int)(capacity * 0.75);
    }

    private int TryInsert(string key, out bool inserted)
    {
        var entries = _entries;
        var cap = entries.Length;
        var hash = (key.GetHashCode() & 0x7FFFFFFF) % cap;

        for (var i = 0; i < cap; i++)
        {
            var index = (hash + i) % cap;
            var entry = entries[index];

            var existing = Volatile.Read(ref entry.Key);
            if (existing == null || existing == DeletedMarker)
            {
                if (Interlocked.CompareExchange(ref entry.Key, key, existing) == existing)
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

            if (existing == null || (existing != key && !existing.Equals(key))) continue;

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
        var hash = (key.GetHashCode() & 0x7FFFFFFF) % cap;

        for (var i = 0; i < cap; i++)
        {
            var index = (hash + i) % cap;
            var entry = entries[index];

            var existing = Volatile.Read(ref entry.Key);
            if (existing == null)
                break;

            if (existing == DeletedMarker || (existing != key && !existing.Equals(key))) continue;

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
            if (entry.Key == null || entry.Key == DeletedMarker)
                continue;

            var hash = (entry.Key.GetHashCode() & 0x7FFFFFFF) % newCap;
            for (var i = 0; i < newCap; i++)
            {
                var index = (hash + i) % newCap;
                var newEntry = newEntries[index];

                if (Volatile.Read(ref newEntry.Key) == null)
                {
                    newEntry.Key = entry.Key;
                    newEntry.Id = entry.Id;
                    break;
                }
            }
        }

        Interlocked.CompareExchange(ref _entries, newEntries, current);
        _threshold = (int)(newCap * 0.75);
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
            if (v != null)
            {
                value = v;
                return true;
            }
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