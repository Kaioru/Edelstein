using System;
using System.Collections.Generic;

namespace Edelstein.Network.Packets
{
    public interface IPacket : IPacketRead, IPacketWrite, IDisposable
    {
        byte[] Buffer { get; }
        int Length { get; }
    }
}