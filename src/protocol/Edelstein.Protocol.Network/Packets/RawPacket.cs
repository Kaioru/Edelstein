using System;
using System.IO;

namespace Edelstein.Protocol.Network.Packets;

public class RawPacket(
    ReadOnlyMemory<byte> buffer
) : IRawPacket
{
    public ReadOnlyMemory<byte> Buffer { get; } = buffer;

    public void DispatchTo(Stream output)
        => output.Write(buffer.Span);
}
