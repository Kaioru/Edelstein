using System;

namespace Edelstein.Protocol.Network.Packets;

public interface IRawPacket : IDisposable, IDispatchable
{
    ReadOnlyMemory<byte> Buffer { get; }
}
