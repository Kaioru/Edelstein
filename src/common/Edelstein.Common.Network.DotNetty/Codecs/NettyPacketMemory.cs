using Microsoft.IO;

namespace Edelstein.Common.Network.DotNetty.Codecs;

internal static class NettyPacketMemory
{
    internal static readonly RecyclableMemoryStreamManager Shared = new(new RecyclableMemoryStreamManager.Options
    {
        BlockSize = 64,
        LargeBufferMultiple = 256,
        ThrowExceptionOnToArray = true
    });
}
