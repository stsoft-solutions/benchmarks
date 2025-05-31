using Core;

namespace Benchmarks.Model;

public record DefaultRecord(
    [FieldMap("f1")] int IntValue,
    [FieldMap("f2")] double DoubleValue,
    [FieldMap("f3")] DateTime DateTimeValue,
    [FieldMap("f4")] char CharValue,
    [FieldMap("f5")] long LongValue,
    [FieldMap("f6")] short ShortValue,
    [FieldMap("f7")] bool BoolValue,
    [FieldMap("f8")] double DoubleValue1);