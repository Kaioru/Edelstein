using System;
using System.Threading.Tasks;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public class Socket : ISocket
    {
        public static readonly AttributeKey<ISocket> Key = AttributeKey<ISocket>.ValueOf("Socket");

        private readonly IChannel _channel;

        public uint SeqSend { get; set; }
        public uint SeqRecv { get; set; }
        public bool EncryptData => true;

        public Socket(IChannel channel, uint seqSend, uint seqRecv)
        {
            _channel = channel;
            SeqSend = seqSend;
            SeqRecv = seqRecv;
        }

        public async Task SendPacket(IPacket packet)
        {
            if (_channel.Open)
                await _channel.WriteAndFlushAsync(packet);
        }

        public Task Close() => _channel.DisconnectAsync();
    }
}