using BenchmarkDotNet.Attributes;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Benchmarks.Packets;

public partial class Packet_RawVsStructured
{
    [Benchmark]
    public Span<byte> Raw()
    {
        using var stream = manager.GetStream();
        using var writer = new RawPacketWriter()
            .WriteBytes(data);
        
        writer.DispatchTo(stream);
        return stream.GetSpan();
    }
}
