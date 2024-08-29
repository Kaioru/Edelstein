using BenchmarkDotNet.Attributes;
using Microsoft.IO;

namespace Edelstein.Benchmarks.Packets;

[MemoryDiagnoser]
public partial class Packet_RawVsStructured
{
    private const int N = 10000;
    private readonly byte[] data;
    
    private readonly RecyclableMemoryStreamManager manager = new(new RecyclableMemoryStreamManager.Options
    {
        BlockSize = 256,
        ThrowExceptionOnToArray = true
    });
    
    public Packet_RawVsStructured()
    {
        data = new byte[N];
        new Random(42).NextBytes(data);
    }
}
