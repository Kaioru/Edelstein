using System;
using System.Collections.Generic;

namespace Edelstein.Network.Packets
{
    public interface IPacket : IDisposable
    {
        byte[] Buffer { get; }
        int Length { get; }

        IPacket Encode<T>(T value);
        IPacket EncodeFixedString(string value, int length);
        T Decode<T>();
        IEnumerable<byte> DecodeFixedLength(int length);
    }
}