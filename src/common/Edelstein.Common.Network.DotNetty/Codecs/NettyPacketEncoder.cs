using System;
using System.Buffers;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Common.Crypto;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Buffers;

namespace Edelstein.Common.Network.DotNetty.Codecs;

public class NettyPacketEncoder(
    TransportVersion transportVersion,
    AESCipher aesCipher,
    IGCipher igCipher
) : MessageToByteEncoder<IPacket>
{

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
            var rawSeq = (short)(seqSend >> 16 ^ -(transportVersion.Major + 1));

            if (socket.IsDataEncrypted)
            {
                ShandaCipher.EncryptTransform(buffer, dataLen);
                aesCipher.Transform(buffer, dataLen, seqSend);
            }

            output.WriteShortLE(rawSeq);
            output.WriteShortLE(dataLen ^ rawSeq);
            output.WriteBytes(buffer, 0, dataLen);

            socket.SeqSend = igCipher.Hash(seqSend, 4, 0);
        }
        else
        {
            output.WriteShortLE(dataLen);
            output.WriteBytes(buffer, 0, dataLen);
        }
        
        ArrayPool<byte>.Shared.Return(buffer);
    }
}
