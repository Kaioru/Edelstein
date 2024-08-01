using System;

namespace Edelstein.Protocol.Network.Packets;

public interface IRawPacket : IDispatchable
{
    ReadOnlyMemory<byte> Buffer { get; }
}
