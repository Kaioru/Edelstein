using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Ciphers;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Pipeline
{
    public class NettyPacketEncoder : MessageToByteEncoder<IPacket>
    {
        private readonly ITransport _transport;

        private readonly AESCipher _aesCipher;
        private readonly IGCipher _igCipher;

        public NettyPacketEncoder(
            ITransport transport,
            AESCipher aesCipher,
            IGCipher igCipher
        )
        {
            _transport = transport;
            _aesCipher = aesCipher;
            _igCipher = igCipher;
        }

        protected override void Encode(IChannelHandlerContext context, IPacket message, IByteBuffer output)
        {
            var socket = context.Channel.GetAttribute(NettyAttributes.SocketKey).Get();
            var dataLen = (short)message.Buffer.Length;
            var buffer = message.Buffer.ToArray();

            if (socket != null)
            {
                var seqSend = socket.SeqSend;
                var rawSeq = (short)((seqSend >> 16) ^ -(_transport.Version + 1));

                if (socket.EncryptData)
                {
                    dataLen ^= rawSeq;

                    ShandaCipher.EncryptTransform(buffer);
                    _aesCipher.Transform(buffer, seqSend);
                }

                output.WriteShortLE(rawSeq);
                output.WriteShortLE(dataLen);
                output.WriteBytes(buffer);

                socket.SeqSend = _igCipher.Hash(seqSend, 4, 0);
            }
            else
            {
                output.WriteShortLE(dataLen);
                output.WriteBytes(buffer);
            }
        }
    }
}