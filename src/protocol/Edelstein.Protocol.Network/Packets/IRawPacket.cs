using System;

namespace Edelstein.Protocol.Network.Packets;

public interface IRawPacket : IDisposable
{
    int Length { get; }
    Memory<byte> Buffer { get; }
}
