using System.Buffers.Binary;

namespace Edelstein.Plugin.Rue.ClientAnalysis.NativeTypes;

public sealed class ZArray<T>
{
    public static IntPtr Create(NativeAllocator allocator, IReadOnlyList<T> values)
    {
        if (values.Count == 0)
            return IntPtr.Zero;

        if (typeof(T) == typeof(int))
            return CreateIntArray(allocator, (IReadOnlyList<int>)(object)values);

        if (typeof(T) == typeof(ZXString))
            return CreateZXStringArray(allocator, (IReadOnlyList<ZXString>)(object)values);

        throw new NotSupportedException($"ZArray<{typeof(T).Name}> is not supported");
    }

    private static IntPtr CreateIntArray(NativeAllocator allocator, IReadOnlyList<int> values)
    {
        var count = values.Count;
        var layout = new ZArrayLayout(count);
        var basePtr = allocator.Allocate(layout.TotalBytes);
        if (basePtr == IntPtr.Zero)
            return IntPtr.Zero;

        var buffer = new byte[layout.TotalBytes];
        var span = buffer.AsSpan();
        BinaryPrimitives.WriteInt32LittleEndian(span[..TypeSizes.Int32], count);

        for (var i = 0; i < count; i++)
        {
            var elementOffset = ZArrayLayout.PayloadOffset + (i * TypeSizes.Int32);
            BinaryPrimitives.WriteInt32LittleEndian(
                span.Slice(elementOffset, TypeSizes.Int32),
                values[i]);
        }

        if (!allocator.WriteBytes(basePtr, buffer))
            return IntPtr.Zero;

        return IntPtr.Add(basePtr, ZArrayLayout.PayloadOffset);
    }

    private static IntPtr CreateZXStringArray(NativeAllocator allocator, IReadOnlyList<ZXString> values)
    {
        var count = values.Count;
        var layout = new ZArrayLayout(count);
        var basePtr = allocator.Allocate(layout.TotalBytes);
        if (basePtr == IntPtr.Zero)
            return IntPtr.Zero;

        var buffer = new byte[layout.TotalBytes];
        var span = buffer.AsSpan();
        BinaryPrimitives.WriteInt32LittleEndian(span[..TypeSizes.Int32], count);

        for (var i = 0; i < count; i++)
        {
            var elementOffset = ZArrayLayout.PayloadOffset + (i * TypeSizes.Int32);
            BinaryPrimitives.WriteInt32LittleEndian(
                span.Slice(elementOffset, TypeSizes.Int32),
                values[i].Pointer.ToInt32());
        }

        if (!allocator.WriteBytes(basePtr, buffer))
            return IntPtr.Zero;

        return IntPtr.Add(basePtr, ZArrayLayout.PayloadOffset);
    }

    internal readonly ref struct ZArrayLayout(int Count)
    {
        public const int CountOffset = 0;
        public const int HeaderBytes = TypeSizes.Int32;
        public const int PayloadOffset = HeaderBytes;

        public int TotalBytes => HeaderBytes + (Count * TypeSizes.Int32);
    }
}
