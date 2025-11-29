using System.Collections;
using System.Collections;

namespace ChunkEnumerableBenchmark;

/// <summary>
///     A struct that represents the entire sequence split into chunks.
/// </summary>
public readonly struct ChunkEnumerable<T> : IEnumerable<Chunk<T>>

{
    private readonly IEnumerable<T> _source;
    private readonly int _chunkSize;

    public ChunkEnumerable(IEnumerable<T> source, int chunkSize)
    {
        _source = source;
        _chunkSize = chunkSize;
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(_source.GetEnumerator(), _chunkSize);
    }

    IEnumerator<Chunk<T>> IEnumerable<Chunk<T>>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public struct Enumerator : IEnumerator<Chunk<T>>
    {
        private IEnumerator<T>? _source;
        private readonly int _chunkSize;
        private Chunk<T> _current;
        private readonly ChunkContext _context;

        internal Enumerator(IEnumerator<T> source, int chunkSize)
        {
            _source = source;
            _chunkSize = chunkSize;
            _current = default;
            _context = new ChunkContext();
        }

        public Chunk<T> Current => _current;
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_source is null)
                return false;

            // Drain remaining elements from the previous chunk if any
            while (_context.Remaining > 0)
            {
                if (!_source.MoveNext())
                {
                    _context.Remaining = 0;
                    break;
                }
                _context.Remaining--;
            }

            // Try to advance the source. If false, we're done.
            if (!_source.MoveNext())
            {
                _source.Dispose();
                _source = null;
                return false;
            }

            // Reset context for the new chunk
            // We already consumed 1 element (the first one)
            _context.Remaining = _chunkSize - 1;

            // Create a new chunk that “starts” with the current element.
            _current = new Chunk<T>(_source, _context);
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            _source?.Dispose();
            _source = null;
        }
    }
}

internal sealed class ChunkContext
{
    public int Remaining;
}