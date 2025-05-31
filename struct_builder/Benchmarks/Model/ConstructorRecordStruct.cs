using System.Runtime.InteropServices;
using Core;

namespace Benchmarks.Model;

[StructLayout(LayoutKind.Sequential, Pack = 0)]
public readonly record struct ConstructorRecordStruct(
    [FieldMap("f1")] int IntValue,
    [FieldMap("f2")] double DoubleValue,
    [FieldMap("f3")] DateTime DateTimeValue,
    [FieldMap("f4")] char CharValue,
    [FieldMap("f5")] long LongValue,
    [FieldMap("f6")] short ShortValue,
    [FieldMap("f7")] bool BoolValue,
    [FieldMap("f8")] double DoubleValue1);