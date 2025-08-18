using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark.StringPools;

/// <summary>
/// A striped + sharded string pool implementation.
/// - String->Id map is sharded by string hash to reduce contention.
/// - Id->String map is sharded by id hash for O(1) reverse lookups.
/// - Ids are assigned from a single global counter starting at 1.
/// </summary>
public sealed class StringPoolStripedSharded : IStringPool
{
    private sealed class StringShard
    {
        public readonly object Lock = new();
        public readonly Dictionary<string, int> StringToId;

        public StringShard(int capacity)
        {
            StringToId = new Dictionary<string, int>(capacity);
        }
    }

    private sealed class IdShard
    {
        public readonly object Lock = new();
        public readonly Dictionary<int, string> IdToString;

        public IdShard(int capacity)
        {
            IdToString = new Dictionary<int, string>(capacity);
        }
    }

    private readonly StringShard[] _stringShards;
    private readonly IdShard[] _idShards;
    private int _nextId = 1;

    public int ShardCount { get; }

    public StringPoolStripedSharded(int initialCapacity, int shardCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(shardCount);
        ArgumentOutOfRangeException.ThrowIfNegative(initialCapacity);

        ShardCount = shardCount;

        // Distribute initial capacity approximately evenly per shard
        var perShardCapacity = Math.Max(1, initialCapacity / Math.Max(1, shardCount));

        _stringShards = new StringShard[shardCount];
        _idShards = new IdShard[shardCount];
        for (var i = 0; i < shardCount; i++)
        {
            _stringShards[i] = new StringShard(perShardCapacity);
            _idShards[i] = new IdShard(perShardCapacity);
        }
    }

    public StringPoolStripedSharded(int shardCount) : this(0, shardCount)
    {
    }

    public StringPoolStripedSharded() : this(Environment.ProcessorCount)
    {
    }

    private int GetStringShardIndex(string value)
    {
        // Make positive and map to [0..ShardCount)
        var h = value.GetHashCode() & 0x7FFFFFFF;
        return h % ShardCount;
    }

    private int GetIdShardIndex(int id)
    {
        var h = id & 0x7FFFFFFF;
        return h % ShardCount;
    }

    public int GetId(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var sIdx = GetStringShardIndex(value);
        var sShard = _stringShards[sIdx];

        // First, fast-path under string-shard lock
        lock (sShard.Lock)
        {
            if (sShard.StringToId.TryGetValue(value, out var existingId))
                return existingId;

            // Create new id
            var id = Interlocked.Increment(ref _nextId) - 1;

            // Put into string -> id first
            sShard.StringToId[value] = id;

            // Then into id -> string in the corresponding id-shard
            var iIdx = GetIdShardIndex(id);
            var iShard = _idShards[iIdx];

            lock (iShard.Lock)
            {
                // Edge case: rare race if another thread mapped same string (shouldn't happen because we guard by value's shard lock)
                iShard.IdToString[id] = value;
            }

            return id;
        }
    }

    public bool TryGetString(int id, [MaybeNullWhen(false)] out string value)
    {
        var iIdx = GetIdShardIndex(id);
        var iShard = _idShards[iIdx];
        lock (iShard.Lock)
        {
            return iShard.IdToString.TryGetValue(id, out value);
        }
    }

    public void Clear()
    {
        // Lock all shards in a consistent order to avoid deadlocks
        // 1) Lock all string shards, then clear
        foreach (var s in _stringShards)
        {
            lock (s.Lock)
            {
                s.StringToId.Clear();
            }
        }

        // 2) Lock all id shards, then clear
        foreach (var i in _idShards)
        {
            lock (i.Lock)
            {
                i.IdToString.Clear();
            }
        }

        // 3) Reset id counter
        Interlocked.Exchange(ref _nextId, 1);
    }
}