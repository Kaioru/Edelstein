using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Common.Cryptography;
using Edelstein.Protocol.Network.Messaging;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Codecs;

public class NettyPacketEncoder : MessageToByteEncoder<IPacket>
{
    private readonly AESCipher _aesCipher;
    private readonly IGCipher _igCipher;
    private readonly ITransport _transport;

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

    protected override void Encode(
        IChannelHandlerContext context,
        IPacket message,
        IByteBuffer output
    )
    {
        var socket = context.Channel.GetAttribute(NettyAttributes.SocketKey).Get();
        var buffer = message.Buffer.ToArray();
        var length = buffer.Length;
        var large = length > 0xFF00;

        if (socket != null)
        {
            var seqSend = socket.SeqSend;
            var rawSeq = (short)((seqSend >> 16) ^ -(_transport.Version + 1));
            var dataLen = length;

            if (socket.IsDataEncrypted)
            {
                dataLen ^= rawSeq;

                _aesCipher.Transform(buffer, seqSend);
            }

            output.WriteShortLE(rawSeq);

            if (large)
            {
                output.WriteShortLE(0xFFFF ^ rawSeq);
                output.WriteIntLE(dataLen);
            }
            else
            {
                output.WriteShortLE(dataLen);
            }

            output.WriteBytes(buffer);

            socket.SeqSend = _igCipher.Hash(seqSend, 4, 0);
        }
        else
        {
            output.WriteShortLE(length);
            output.WriteBytes(buffer);
        }
    }
}
