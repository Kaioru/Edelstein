using System.Buffers;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Utilities.Packets;

public class Packet : IPacket
{
    public Packet(byte[] buffer)
    {
        Length = buffer.Length;
        Buffer = ArrayPool<byte>.Shared.Rent(Length);
        
        Array.Copy(buffer, Buffer, Length);
    }
    
    public Packet(Stream stream)
    {
        Length = (int)stream.Length;
        Buffer = ArrayPool<byte>.Shared.Rent(Length);

        var position = stream.Position;

        stream.Position = 0;
        _ = stream.Read(Buffer, 0, Length);
        stream.Position = position;
    }
    
    public int Length { get; }
    public byte[] Buffer { get; }
    
    public void Dispose() 
        => ArrayPool<byte>.Shared.Return(Buffer);
}
