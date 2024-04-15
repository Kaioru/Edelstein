using Microsoft.IO;

namespace Edelstein.Common.Utilities.Buffers;

internal static class PacketMemoryStream
{
    internal static readonly RecyclableMemoryStreamManager Shared = new();
}
