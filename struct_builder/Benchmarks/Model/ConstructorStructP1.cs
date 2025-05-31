using System.Runtime.InteropServices;
using Core;

namespace Benchmarks.Model;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct ConstructorStructP1
{
    public readonly int IntValue;
    public readonly double DoubleValue;
    public readonly DateTime DateTimeValue;
    public readonly char CharValue;
    public readonly long LongValue;
    public readonly short ShortValue;
    public readonly bool BoolValue;
    public readonly double DoubleValue1;

    public ConstructorStructP1(
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