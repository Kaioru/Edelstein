using System.Text;
using BinarySerialization;

namespace Edelstein.Protocol.Network.Packets;

internal static class StructuredPacketSerializer
{
    internal static readonly BinarySerializer Shared = new()
    {
        Encoding = Encoding.ASCII,
        Endianness = Endianness.Little
    };
}
