using Microsoft.IO;

namespace Edelstein.Protocol.Network.Packets;

internal static class RawPacketMemory
{
    internal static readonly RecyclableMemoryStreamManager Shared = new(new RecyclableMemoryStreamManager.Options
    {
        BlockSize = 256,
        ThrowExceptionOnToArray = true
    });
}
