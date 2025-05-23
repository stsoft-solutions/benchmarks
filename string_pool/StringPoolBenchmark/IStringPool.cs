using System.Diagnostics.CodeAnalysis;

namespace StringPoolBenchmark;

public interface IStringPool
{
    int GetId(string value);
    bool TryGetString(int id, [MaybeNullWhen(false)] out string value);
    void Clear();
}