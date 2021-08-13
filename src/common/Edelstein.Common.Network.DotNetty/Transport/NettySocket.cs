using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Transport
{
    public class NettySocket : ISocket
    {
        private readonly IChannel _channel;

        public string ID => _channel.Id.AsLongText();
        public uint SeqSend { get; set; }
        public uint SeqRecv { get; set; }
        public bool EncryptData { get; init; }

        public NettySocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            bool encryptData = true
        )
        {
            Debug.Assert(channel != null);

            _channel = channel;

            SeqSend = seqSend;
            SeqRecv = seqRecv;
            EncryptData = encryptData;
        }

        public async Task Dispatch(IPacket packet)
        {
            if (_channel.IsWritable)
                await _channel.WriteAndFlushAsync(packet);
        }

        public Task Disconnect() => _channel.DisconnectAsync();

    }
}
