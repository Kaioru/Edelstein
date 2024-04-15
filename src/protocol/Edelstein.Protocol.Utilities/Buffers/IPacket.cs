using System;

namespace Edelstein.Protocol.Utilities.Buffers;

public interface IPacket : IDisposable
{
    int Length { get; }
    byte[] Buffer { get; }
}
