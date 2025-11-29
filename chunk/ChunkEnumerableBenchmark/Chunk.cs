using System.Collections;
using System.Diagnostics;

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
    private readonly ChunkContext _context;

    internal Chunk(IEnumerator<T> source, ChunkContext context)
    {
        _source = source;
        _context = context;
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(_source, _context);
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
        private readonly ChunkContext _context;
        private bool _first;
        private T _current;

        internal Enumerator(IEnumerator<T> source, ChunkContext context)
        {
            _source = source;
            _context = context;
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
                // One element already taken (accounted for in context.Remaining initialization)
                return true;
            }

            if (_context.Remaining <= 0 || !_source.MoveNext()) return false;

            // Continue for the rest of the chunk.
            _current = _source.Current;
            _context.Remaining--;
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