using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Network.Ciphers;
using Edelstein.Network.Packets;

namespace Edelstein.Network.Codecs
{
    public class PacketEncoder : MessageToByteEncoder<IPacket>
    {
        private readonly short _version;

        public PacketEncoder(short version)
            => _version = version;

        protected override void Encode(IChannelHandlerContext context, IPacket message, IByteBuffer output)
        {
            var socket = context.Channel.GetAttribute(Socket.Key).Get();
            var dataLen = (short) message.Length;
            var buffer = message.Buffer;

            if (socket != null)
            {
                var seqSend = socket.SeqSend;
                var rawSeq = (short) ((seqSend >> 16) ^ -(_version + 1));

                if (socket.EncryptData)
                {
                    dataLen ^= rawSeq;

                    ShandaCipher.EncryptTransform(buffer);
                    AESCipher.Transform(buffer, seqSend);
                }

                output.WriteShortLE(rawSeq);
                output.WriteShortLE(dataLen);
                output.WriteBytes(buffer);

                socket.SeqSend = IGCipher.InnoHash(seqSend, 4, 0);
            }
            else
            {
                output.WriteShortLE(dataLen);
                output.WriteBytes(buffer);
            }
        }
    }
}