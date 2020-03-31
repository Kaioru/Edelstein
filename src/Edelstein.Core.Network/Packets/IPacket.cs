using System;

namespace Edelstein.Network.Packets
{
    public interface IPacket : IPacketRead, IPacketWrite, IDisposable
    {
        byte[] Buffer { get; }
        int Length { get; }
    }
}