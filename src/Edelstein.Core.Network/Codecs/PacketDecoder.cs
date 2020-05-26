using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Core.Network.Ciphers;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Network.Codecs
{
    public class PacketDecoder : ReplayingDecoder<PacketState>
    {
        private readonly short _version;

        private short _sequence;
        private short _length;

        public PacketDecoder(short version)
            : base(PacketState.Header)
            => _version = version;

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            var socket = context.Channel.GetAttribute(Socket.Key).Get();

            try
            {
                switch (State)
                {
                    case PacketState.Header:
                        if (socket != null)
                        {
                            var sequence = input.ReadShortLE();
                            var length = input.ReadShortLE();

                            if (socket.EncryptData) length ^= sequence;

                            _sequence = sequence;
                            _length = length;
                        }
                        else _length = input.ReadShortLE();

                        Checkpoint(PacketState.Payload);
                        return;
                    case PacketState.Payload:
                        var buffer = new byte[_length];

                        input.ReadBytes(buffer);
                        Checkpoint(PacketState.Header);

                        if (_length < 0x2) return;

                        if (socket != null)
                        {
                            var seqRecv = socket.SeqRecv;
                            var version = (short) (seqRecv >> 16) ^ _sequence;

                            if (!(version == -(_version + 1) ||
                                  version == _version)) return;

                            if (socket.EncryptData)
                            {
                                AESCipher.Transform(buffer, seqRecv);
                                ShandaCipher.DecryptTransform(buffer);
                            }

                            socket.SeqRecv = IGCipher.InnoHash(seqRecv, 4, 0);
                        }

                        output.Add(new InPacket(buffer));
                        return;
                }
            }
            catch (IndexOutOfRangeException)
            {
                RequestReplay();
            }
        }
    }
}