using System.Collections;
using System.Diagnostics;

namespace ChunkEnumerableBenchmark;

/// <summary>
///     Represents a single chunk of the source sequence.
///     Note that this chunk is a “live view” over the underlying enumerator.
/// </summary>
public readonly struct Chunk<T> : IEnumerable<T>
{
    // Note: We keep a reference to the underlying enumerator.
    // This design requires that the caller fully enumerates one chunk
    // before calling MoveNext on the outer enumerator.
    private readonly IEnumerator<T> _source;
    private readonly int _chunkSize;

    internal Chunk(IEnumerator<T> source, int chunkSize)
    {
        _source = source;
        _chunkSize = chunkSize;
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(_source, _chunkSize);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public struct Enumerator : IEnumerator<T>
    {
        private readonly IEnumerator<T> _source;
        private int _remaining;
        private bool _first;
        private T _current;

        internal Enumerator(IEnumerator<T> source, int chunkSize)
        {
            _source = source;
            _remaining = chunkSize;
            _first = true;
            _current = default!;
        }

        public T Current => _current;

        object IEnumerator.Current
        {
            get
            {
                Debug.Assert(Current != null, nameof(Current) + " != null");
                return Current;
            }
        }

        public bool MoveNext()
        {
            // The first call to MoveNext should yield the element already
            // in _source.Current (set by the outer enumerator).
            if (_first)
            {
                _first = false;
                _current = _source.Current;
                _remaining--; // one element already taken
                return true;
            }

            if (_remaining <= 0 || !_source.MoveNext()) return false;

            // Continue for the rest of the chunk.
            _current = _source.Current;
            _remaining--;
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
        }
    }
}