using System;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Common.Crypto;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Transports;

namespace Edelstein.Common.Network.DotNetty.Codecs;

public class NettyPacketEncoder(
    TransportVersion transportVersion,
    CipherAES cipherAES,
    CipherIG cipherIG
) : MessageToByteEncoder<IDispatchable>
{
    protected override void Encode(IChannelHandlerContext context, IDispatchable message, IByteBuffer output)
    {
        var socket = context.Channel.GetAttribute(NettyAttributes.SocketKey).Get();
        using var stream = NettyPacketMemory.Shared.GetStream();
        
        message.DispatchTo(stream);
        stream.Position = 0;
        
        var dataLen = (int)stream.Length;
        var buffer = stream
            .GetBuffer()
            .AsSpan()[..dataLen];

        if (socket != null)
        {
            var seqSend = socket.SeqSend;
            var rawSeq = (short)(seqSend >> 16 ^ -(transportVersion.Major + 1));

            if (socket.IsDataEncrypted)
            {
                CipherShanda.EncryptTransform(buffer, dataLen);
                cipherAES.Transform(buffer, dataLen, seqSend);
            }

            output.WriteShortLE(rawSeq);
            output.WriteShortLE(dataLen ^ rawSeq);
            output.WriteBytes(buffer);

            socket.SeqSend = cipherIG.Hash(seqSend, 4, 0);
        }
        else
        {
            output.WriteShortLE(dataLen);
            output.WriteBytes(buffer);
        }
    }
}
