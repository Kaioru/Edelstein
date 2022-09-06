using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Common.Crypto;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Util.Buffers.Packets;

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
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
        _aesCipher = aesCipher ?? throw new ArgumentNullException(nameof(aesCipher));
        _igCipher = igCipher ?? throw new ArgumentNullException(nameof(igCipher));
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

        var isLarge = length > 0xFF00;

        if (socket != null)
        {
            var seqSend = socket.SeqSend;
            var rawSeq = (short)((seqSend >> 16) ^ -(_transport.Version + 1));
            var dataLen = length;

            if (socket.IsDataEncrypted)
            {
                dataLen ^= rawSeq;

                // ShandaCipher.EncryptTransform(buffer);
                _aesCipher.Transform(buffer, seqSend);
            }

            output.WriteShortLE(rawSeq);

            if (isLarge)
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
