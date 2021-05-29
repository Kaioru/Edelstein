using System;

namespace Edelstein.Protocol.Network
{
    public interface IPacket
    {
        ReadOnlySpan<byte> Buffer { get; }
    }
}
