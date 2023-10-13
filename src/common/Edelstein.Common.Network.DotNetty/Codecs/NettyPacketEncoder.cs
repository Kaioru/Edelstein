using System.Buffers;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Common.Crypto;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Network.DotNetty.Codecs;

public class NettyPacketEncoder : MessageToByteEncoder<IPacket>
{
    private readonly AESCipher _aesCipher;
    private readonly IGCipher _igCipher;
    private readonly TransportVersion _version;

    public NettyPacketEncoder(
        TransportVersion version,
        AESCipher aesCipher,
        IGCipher igCipher
    )
    {
        _version = version;
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
        var dataLen = message.Length;
        var buffer = ArrayPool<byte>.Shared.Rent(dataLen);
        
        Array.Copy(message.Buffer, buffer, dataLen);
        
        if (socket != null)
        {
            var seqSend = socket.SeqSend;
            var rawSeq = (short)(seqSend >> 16 ^ -(_version.Major + 1));

            if (socket.IsDataEncrypted)
            {
                ShandaCipher.EncryptTransform(buffer, dataLen);
                _aesCipher.Transform(buffer, dataLen, seqSend);
            }

            output.WriteShortLE(rawSeq);
            output.WriteShortLE(dataLen ^ rawSeq);
            output.WriteBytes(buffer, 0, dataLen);

            socket.SeqSend = _igCipher.Hash(seqSend, 4, 0);
        }
        else
        {
            output.WriteShortLE(dataLen);
            output.WriteBytes(buffer, 0, dataLen);
        }
        
        ArrayPool<byte>.Shared.Return(buffer);
    }
}
