using System.Net;
using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Messaging;

namespace Edelstein.Common.Network.DotNetty;

public class NettySocket : ISocket
{
    private readonly IChannel _channel;

    public string ID => _channel.Id.AsLongText();

    public EndPoint AddressLocal => _channel.LocalAddress;
    public EndPoint AddressRemote => _channel.RemoteAddress;

    public bool IsDataEncrypted { get; }
    public uint SeqSend { get; set; }
    public uint SeqRecv { get; set; }

    public async Task Dispatch(IPacket packet)
    {
        if (_channel.IsWritable)
            await _channel.WriteAndFlushAsync(packet);
    }

    public Task Close() => _channel.DisconnectAsync();

    public NettySocket(IChannel channel, uint seqSend, uint seqRecv, bool isDataEncrypted = true)
    {
        _channel = channel;
        IsDataEncrypted = isDataEncrypted;
        SeqSend = seqSend;
        SeqRecv = seqRecv;
    }
}
