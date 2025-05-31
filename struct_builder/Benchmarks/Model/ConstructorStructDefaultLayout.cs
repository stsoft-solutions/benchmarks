using Core;

namespace Benchmarks.Model;

/// <summary>
/// Represents a simple structure with integer and double values.
/// </summary>
public readonly struct ConstructorStructDefaultLayout
{
    public readonly int IntValue;
    public readonly double DoubleValue;
    public readonly DateTime DateTimeValue;
    public readonly char CharValue;
    public readonly long LongValue;
    public readonly short ShortValue;
    public readonly bool BoolValue;
    public readonly double DoubleValue1;

    public ConstructorStructDefaultLayout(
        [FieldMap("f1")] int intValue,
        [FieldMap("f2")] double doubleValue,
        [FieldMap("f3")] DateTime dateTimeValue,
        [FieldMap("f4")] char charValue,
        [FieldMap("f5")] long longValue,
        [FieldMap("f6")] short shortValue,
        [FieldMap("f7")] bool boolValue,
        [FieldMap("f8")] double doubleValue1)
    {
        IntValue = intValue;
        DoubleValue = doubleValue;
        DateTimeValue = dateTimeValue;
        CharValue = charValue;
        LongValue = longValue;
        ShortValue = shortValue;
        BoolValue = boolValue;
        DoubleValue1 = doubleValue1;
    }
}