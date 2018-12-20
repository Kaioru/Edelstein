using System;
using System.Threading.Tasks;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public abstract class AbstractSocket : ISocket
    {
        public static readonly AttributeKey<ISocket> SocketKey = AttributeKey<ISocket>.ValueOf("Socket");

        private readonly IChannel _channel;

        public uint SeqSend { get; set; }
        public uint SeqRecv { get; set; }
        public bool EncryptData => true;

        public object LockSend { get; }
        public object LockRecv { get; }

        public AbstractSocket(IChannel channel, uint seqSend, uint seqRecv)
        {
            _channel = channel;
            SeqSend = seqSend;
            SeqRecv = seqRecv;
            LockSend = new object();
            LockRecv = new object();
        }

        public abstract Task OnPacket(IPacket packet);
        public abstract Task OnDisconnect();
        public abstract Task OnException(Exception exception);

        public Task Disconnect()
            => _channel.DisconnectAsync();

        public Task SendPacket(IPacket packet)
            => _channel.WriteAndFlushAsync(packet);
    }
}