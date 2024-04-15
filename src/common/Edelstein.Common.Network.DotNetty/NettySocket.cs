using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Buffers;

namespace Edelstein.Common.Network.DotNetty;

public class NettySocket(
    IChannel channel, 
    uint seqSend,
    uint seqRecv, 
    bool isDataEncrypted = true
)
    : ISocket
{

    public string ID => channel.Id.AsLongText();

    public EndPoint AddressLocal => channel.LocalAddress;
    public EndPoint AddressRemote => channel.RemoteAddress;

    public uint SeqSend { get; set; } = seqSend;
    public uint SeqRecv { get; set; } = seqRecv;

    public bool IsDataEncrypted { get; } = isDataEncrypted;

    public DateTime LastAliveSent { get; set; }
    public DateTime LastAliveRecv { get; set; }

    public async Task Dispatch(IPacket packet)
    {
        if (channel.IsWritable)
            await channel.WriteAndFlushAsync(packet);
    }

    public Task Close() => channel.DisconnectAsync();
}
