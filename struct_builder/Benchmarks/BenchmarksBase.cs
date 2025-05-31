using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Benchmarks.Model;

namespace Benchmarks;

[MemoryDiagnoser]
public abstract class BenchmarksBase
{
    protected Dictionary<int, ConstructorStructDefaultLayout> DictDefault = null!;
    protected Dictionary<int, ConstructorStructP0> DictP0 = null!;
    protected Dictionary<int, ConstructorStructP1> DictP1 = null!;
    protected Dictionary<int, DefaultClass> DictDefaultClass = null!;
    protected Dictionary<int, DefaultRecord> DictDefaultRecord = null!;
    protected List<int> KeysToLookup = null!;
    protected Random Rnd = null!;
    protected const int ItemCount = 100_000;
    private const int LookupCount = 10_000;
    [Params(10, 1000, 10_000)] public int DictionarySize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Rnd = new Random(42); // Fixed seed for reproducibility

        // Initialize dictionaries
        DictDefault = new Dictionary<int, ConstructorStructDefaultLayout>(DictionarySize);
        DictP0 = new Dictionary<int, ConstructorStructP0>(DictionarySize);
        DictP1 = new Dictionary<int, ConstructorStructP1>(DictionarySize);
        DictDefaultClass = new Dictionary<int, DefaultClass>();
        DictDefaultRecord = new Dictionary<int, DefaultRecord>();

        // Create a lookup keys list (to ensure fair comparison across benchmarks)
        KeysToLookup = new List<int>(LookupCount);

        // Populate dictionaries with identical data
        for (var i = 0; i < DictionarySize; i++)
        {
            var intValue = Rnd.Next();
            var doubleValue = Rnd.NextDouble() * 100;
            var dateTimeValue = DateTime.Now.AddDays(Rnd.Next(-1000, 1000));
            var charValue = (char)Rnd.Next(65, 90); // A-Z
            var longValue = Rnd.NextInt64();
            var shortValue = (short)Rnd.Next(short.MinValue, short.MaxValue);
            var boolValue = Rnd.Next(2) == 1;

            DictDefault[i] = new ConstructorStructDefaultLayout(
                intValue, doubleValue, dateTimeValue, charValue,
                longValue, shortValue, boolValue, doubleValue);

            DictP0[i] = new ConstructorStructP0(
                intValue, doubleValue, dateTimeValue, charValue,
                longValue, shortValue, boolValue, doubleValue);

            DictP1[i] = new ConstructorStructP1(
                intValue, doubleValue, dateTimeValue, charValue,
                longValue, shortValue, boolValue, doubleValue);

            DictDefaultClass[i] = new DefaultClass(
                intValue, doubleValue, dateTimeValue, charValue,
                longValue, shortValue, boolValue, doubleValue);

            DictDefaultRecord[i] = new DefaultRecord(
                intValue, doubleValue, dateTimeValue, charValue,
                longValue, shortValue, boolValue, doubleValue);

            // Add some keys to a lookup list (every 10th key)
            if (i % 10 == 0 && KeysToLookup.Count < LookupCount) KeysToLookup.Add(i);
        }

        // Fill the remaining lookup keys if needed
        while (KeysToLookup.Count < LookupCount)
        {
            KeysToLookup.Add(Rnd.Next(0, DictionarySize));
        }
    }
}