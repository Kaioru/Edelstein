using System;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Network.Crypto;

namespace Edelstein.Network.Packets.Codecs
{
    public class PacketEncoder : MessageToByteEncoder<Packet>
    {
        protected override void Encode(IChannelHandlerContext context, Packet message, IByteBuffer output)
        {
            var socket = context.Channel.GetAttribute(AbstractSocket.SocketKey).Get();

            if (socket != null)
            {
                lock (socket.LockSend)
                {
                    var seqSend = socket.SeqSend;
                    var rawSeq = (short) ((seqSend >> 16) ^ -(AESCipher.Version + 1));
                    var dataLen = (short) message.Length;
                    var buffer = new byte[dataLen];

                    Array.Copy(message.Buffer, buffer, dataLen);

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
            }
            else
            {
                var length = message.Length;
                var buffer = new byte[length];

                Array.Copy(message.Buffer, buffer, length);

                output.WriteShortLE(length);
                output.WriteBytes(buffer);
            }
        }
    }
}