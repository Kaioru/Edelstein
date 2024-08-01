using System;
using System.Buffers;
using System.IO;
using BinarySerialization;
using CommunityToolkit.HighPerformance.Buffers;
using Microsoft.IO;

namespace Edelstein.Protocol.Network.Packets;

public class RawPacket : IRawPacket
{
    public int Length => Buffer.Length;
    public Memory<byte> Buffer => _owner.Memory;

    private readonly MemoryOwner<byte> _owner;
    
    public RawPacket(MemoryOwner<byte> owner)
    {
        _owner = owner;
    }
    
    public RawPacket(RecyclableMemoryStream stream)
    {
        _owner = MemoryOwner<byte>.Allocate((int)stream.Length);
        stream.GetReadOnlySequence().CopyTo(Buffer.Span);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _owner.Dispose();
    }
}
