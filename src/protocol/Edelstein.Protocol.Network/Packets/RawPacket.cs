using System;
using System.Buffers;
using System.IO;

namespace Edelstein.Protocol.Network.Packets;

public class RawPacket(
    IMemoryOwner<byte> owner
) : IRawPacket
{
    public ReadOnlyMemory<byte> Buffer { get; } = owner.Memory;

    public void DispatchTo(Stream output)
        => output.Write(owner.Memory.Span);
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        owner.Dispose();
    }
}
