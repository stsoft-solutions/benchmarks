using System.Reflection;
using System.Runtime.InteropServices;

namespace Core;

/// <summary>
/// Provides methods to calculate the memory size of objects and types in bytes.
/// </summary>
/// <remarks>
/// Based on the implementation from Alex Pinsker's blog post:
/// https://alexpinsker.blogspot.com/2011/10/what-is-size-of-datetime-type-in-c.html
/// </remarks>
public static class SizeCalculator
{
    private struct TypeSizeProxy<T>
    {
        private T _publicField;

        public TypeSizeProxy(T publicField)
        {
            _publicField = publicField;
        }
    }

    public static int SizeOf<T>()
    {
        return SizeOf(typeof(T));
    }

    public static int SizeOf(Type type)
    {
        try
        {
            return Marshal.SizeOf(type);
        }
        catch (ArgumentException)
        {
            var proxyType = typeof(TypeSizeProxy<>).MakeGenericType(type);
            // Create a new instance proxy type to calculate the size
            var proxyInstance = Activator.CreateInstance(proxyType);
            if (proxyInstance != null) return Marshal.SizeOf(proxyInstance);
            throw new InvalidOperationException($"Unable to calculate size for type {type.FullName}");
        }
    }

    public static int GetSize(this object obj)
    {
        return Marshal.SizeOf(obj);
    }

    public static void PrintStructInfo<T>() where T : struct
    {
        var t = typeof(T);

        Console.WriteLine($"Type: {t.Name}");
        Console.WriteLine($"Size of {t.Name}: {SizeOf<T>()} bytes");
        foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
        {
            var offset = (int)Marshal.OffsetOf(t, field.Name);
            var fieldName = field.Name;
            var size = SizeOf(field.FieldType);

            Console.WriteLine($"{fieldName,-40} Offset: {offset,2} bytes, Size: {size} bytes");
        }
    }
}