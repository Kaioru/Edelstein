using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Transports;

namespace Edelstein.Common.Network.DotNetty;

public class NettySocket(
    IChannel channel, 
    TransportVersion version,
    uint seqSend,
    uint seqRecv, 
    bool isDataEncrypted = true
) : ISocket
{
    public string ID => channel.Id.AsLongText();

    public TransportState State => channel.IsActive ? TransportState.Opened : TransportState.Closed;
    public TransportVersion Version => version;
    
    public EndPoint AddressLocal => channel.LocalAddress;
    public EndPoint AddressRemote => channel.RemoteAddress;

    public uint SeqSend { get; set; } = seqSend;
    public uint SeqRecv { get; set; } = seqRecv;

    public bool IsDataEncrypted { get; } = isDataEncrypted;
    
    public async Task Dispatch(IDispatchable dispatch)
    {
        if (channel.IsWritable)
            await channel.WriteAndFlushAsync(dispatch);
    }

    public Task Close() => channel.DisconnectAsync();
}
