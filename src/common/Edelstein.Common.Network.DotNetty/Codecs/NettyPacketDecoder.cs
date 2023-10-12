using System.Buffers;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Common.Crypto;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Network.Transports;

namespace Edelstein.Common.Network.DotNetty.Codecs;

public class NettyPacketDecoder : ReplayingDecoder<NettyPacketState>
{
    private readonly AESCipher _aesCipher;
    private readonly IGCipher _igCipher;
    private readonly TransportVersion _version;
    private short _length;

    private short _sequence;

    public NettyPacketDecoder(
        TransportVersion version,
        AESCipher aesCipher,
        IGCipher igCipher
    ) : base(NettyPacketState.DecodingHeader)
    {
        _version = version;
        _aesCipher = aesCipher;
        _igCipher = igCipher;
    }

    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        var socket = context.Channel.GetAttribute(NettyAttributes.SocketKey).Get();

        switch (State)
        {
            case NettyPacketState.DecodingHeader:
                if (socket != null)
                {
                    if (input.ReadableBytes < 4)
                    {
                        RequestReplay();
                        return;
                    }

                    var sequence = input.ReadShortLE();
                    var length = input.ReadShortLE();

                    if (socket.IsDataEncrypted) length ^= sequence;

                    _sequence = sequence;
                    _length = length;
                }
                else
                {
                    if (input.ReadableBytes < 2)
                    {
                        RequestReplay();
                        return;
                    }

                    _length = input.ReadShortLE();
                }

                Checkpoint(NettyPacketState.DecodingPayload);
                return;
            case NettyPacketState.DecodingPayload:
                if (input.ReadableBytes < _length)
                {
                    RequestReplay();
                    return;
                }

                var buffer = ArrayPool<byte>.Shared.Rent(_length);

                input.ReadBytes(buffer, 0, _length);
                Checkpoint(NettyPacketState.DecodingHeader);

                if (_length < 0x2) return;

                if (socket != null)
                {
                    var seqRecv = socket.SeqRecv;
                    var version = (short)(seqRecv >> 16) ^ _sequence;

                    if (!(version == -(_version.Major + 1) ||
                          version == _version.Major)) return;

                    if (socket.IsDataEncrypted)
                    {
                        _aesCipher.Transform(buffer, _length, seqRecv);
                        ShandaCipher.DecryptTransform(buffer, _length);
                    }

                    socket.SeqRecv = _igCipher.Hash(seqRecv, 4, 0);
                }

                output.Add(new Packet(buffer));
                ArrayPool<byte>.Shared.Return(buffer);
                return;
        }
    }
}
