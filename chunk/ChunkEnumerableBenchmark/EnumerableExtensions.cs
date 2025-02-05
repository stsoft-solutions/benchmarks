namespace ChunkEnumerableBenchmark;

internal static class EnumerableExtensions
{
    /// <summary>
    ///     Splits the source sequence into chunks of the specified size.
    /// </summary>
    /// <typeparam name="T">Type of the sequence</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="chunkSize">Chunk size</param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> ChunkEnumerable<T>(this IEnumerable<T> source, int chunkSize)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (chunkSize <= 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));

        using var enumerator = source.GetEnumerator();

        while (enumerator.MoveNext()) yield return GetChunk(enumerator, chunkSize);

        yield break;

        static IEnumerable<T> GetChunk(IEnumerator<T> enumerator, int chunkSize)
        {
            do
            {
                yield return enumerator.Current;
            } while (--chunkSize > 0 && enumerator.MoveNext());
        }
    }

    /// <summary>
    ///     Splits the source sequence into chunks of the specified size. Without using the yield keyword.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="chunkSize"></param>
    /// <returns></returns>
    public static ChunkEnumerable<T> ChunkEnumerableLight<T>(this IEnumerable<T> source, int chunkSize)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (chunkSize <= 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));

        return new ChunkEnumerable<T>(source, chunkSize);
    }
}