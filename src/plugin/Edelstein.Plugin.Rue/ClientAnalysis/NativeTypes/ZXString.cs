using System.Buffers.Binary;
using System.Text;

namespace Edelstein.Plugin.Rue.ClientAnalysis.NativeTypes;

public readonly struct ZXString
{
    public IntPtr Pointer { get; }

    private ZXString(IntPtr pointer)
    {
        Pointer = pointer;
    }

    public static ZXString FromPointer(IntPtr pointer) => new(pointer);

    public static ZXString Create(NativeAllocator allocator, string value)
    {
        var input = value ?? string.Empty;
        var payloadBytes = Encoding.ASCII.GetByteCount(input);
        var layout = new ZXStringLayout(payloadBytes);

        var basePtr = allocator.Allocate(layout.TotalBytes);
        if (basePtr == IntPtr.Zero)
            return new ZXString(IntPtr.Zero);

        var buffer = new byte[layout.TotalBytes];
        var span = buffer.AsSpan();
        var refCountSpan = span[..TypeSizes.Int32];
        var lengthSpan = span.Slice(ZXStringLayout.LengthOffset, TypeSizes.Int32);
        BinaryPrimitives.WriteInt32LittleEndian(refCountSpan, 1);
        BinaryPrimitives.WriteInt32LittleEndian(lengthSpan, payloadBytes);
        var payloadSpan = span.Slice(ZXStringLayout.PayloadOffset, payloadBytes);
        Encoding.ASCII.GetBytes(input, payloadSpan);
        span[ZXStringLayout.PayloadOffset + payloadBytes] = 0;

        if (!allocator.WriteBytes(basePtr, buffer))
            return new ZXString(IntPtr.Zero);

        var dataPtr = IntPtr.Add(basePtr, ZXStringLayout.PayloadOffset);
        return new ZXString(dataPtr);
    }

    internal readonly ref struct ZXStringLayout(int PayloadBytes)
    {
        public const int RefCountOffset = 0;
        public const int LengthOffset = TypeSizes.Int32;
        public const int HeaderBytes = TypeSizes.Int32 * 2;
        public const int NullTerminatorBytes = 1;
        public const int PayloadOffset = HeaderBytes;

        public int TotalBytes => HeaderBytes + PayloadBytes + NullTerminatorBytes;
    }
}
