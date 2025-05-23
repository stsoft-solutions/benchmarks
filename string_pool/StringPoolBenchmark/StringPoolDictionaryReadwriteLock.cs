using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark;

public sealed class StringPoolDictionaryReadwriteLock : IStringPool, IDisposable
{
    private readonly Dictionary<int, string> _idToString = new();
    private readonly ReaderWriterLockSlim _lock = new();
    private readonly Dictionary<string, int> _stringToId = new();
    private int _nextId = 1;

    public void Dispose()
    {
        _lock.Dispose();
    }

    public int GetId(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        _lock.EnterUpgradeableReadLock();

        try
        {
            if (_stringToId.TryGetValue(value, out var existingId))
                return existingId;

            _lock.EnterWriteLock();
            try
            {
                if (_stringToId.TryGetValue(value, out existingId))
                    return existingId;

                var id = _nextId++;
                _stringToId[value] = id;
                _idToString[id] = value;
                return id;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        finally
        {
            _lock.ExitUpgradeableReadLock();
        }
    }

    public bool TryGetString(int id, [MaybeNullWhen(false)] out string value)
    {
        _lock.EnterReadLock();
        try
        {
            return _idToString.TryGetValue(id, out value);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _stringToId.Clear();
            _idToString.Clear();
            _nextId = 1;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}