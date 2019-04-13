using System;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Network.Crypto;

namespace Edelstein.Network.Packets.Codecs
{
    public class PacketEncoder : MessageToByteEncoder<IPacket>
    {
        protected override void Encode(IChannelHandlerContext context, IPacket message, IByteBuffer output)
        {
            var socket = context.Channel.GetAttribute(AbstractSocket.SocketKey).Get();
            var dataLen = (short) message.Length;
            var buffer = new byte[dataLen];

            Array.Copy(message.Buffer, buffer, dataLen);

            if (socket != null)
            {
                var seqSend = socket.SeqSend;
                var rawSeq = (short) ((seqSend >> 16) ^ -(95 + 1));

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