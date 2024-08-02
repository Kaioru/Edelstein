using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Transports;

namespace Edelstein.Common.Network.DotNetty.Transports;

public class NettyTransportContext(
    IChannel channel,
    IChannelGroup group,
    TransportVersion version
) : ITransportContext
{
    public string ID => channel.Id.AsLongText();
    
    public TransportState State => channel.IsActive ? TransportState.Opened : TransportState.Closed;
    public TransportVersion Version => version;
    
    public EndPoint AddressLocal => channel.LocalAddress;
    public EndPoint AddressRemote => channel.RemoteAddress;
    
    public Task Dispatch(IDispatchable dispatch)
        => group.WriteAndFlushAsync(dispatch);

    public async Task Close()
    {
        await group.DisconnectAsync();
        await channel.CloseAsync();
    }
}
