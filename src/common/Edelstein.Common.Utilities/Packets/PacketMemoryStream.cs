using Microsoft.IO;

namespace Edelstein.Common.Utilities.Packets;

internal static class PacketMemoryStream
{
    internal static readonly RecyclableMemoryStreamManager Shared = new();
}
