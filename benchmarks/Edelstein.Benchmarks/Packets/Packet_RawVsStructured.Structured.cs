using BenchmarkDotNet.Attributes;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Benchmarks.Packets;

public partial class Packet_RawVsStructured
{
    [Benchmark]
    public Span<byte> Structured()
    {
        using var stream = manager.GetStream();
        using var writer = new RawPacketWriter()
            .WriteStructured(new StructuredPacket
            {
                Data = data
            });
        
        writer.DispatchTo(stream);
        return stream.GetSpan();
    }
}

public record StructuredPacket : StructuredBasePacket
{
    [FieldOrder(0)]
    public required byte[] Data { get; init; }
}
