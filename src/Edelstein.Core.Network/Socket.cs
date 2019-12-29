using System;
using System.Threading;
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
        private readonly SemaphoreSlim _sendLock;

        public uint SeqSend { get; set; }
        public uint SeqRecv { get; set; }
        public long ClientKey { get; set; }
        public bool EncryptData => true;

        public Socket(IChannel channel, uint seqSend, uint seqRecv)
        {
            _channel = channel;
            _sendLock = new SemaphoreSlim(1, 1);
            SeqSend = seqSend;
            SeqRecv = seqRecv;
            ClientKey = new Random().NextLong();
        }

        public async Task SendPacket(IPacket packet)
        {
            await _sendLock.WaitAsync();

            try
            {
                if (_channel.Open)
                    await _channel.WriteAndFlushAsync(packet);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        public Task Close() => _channel.DisconnectAsync();
    }
}