using System.Collections.Generic;
using Edelstein.Network.Packet;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Field.Movement
{
    public class MovementPath
    {
        private readonly ICollection<Movement> _path;

        public short X { get; set; }
        public short Y { get; set; }
        public short VX { get; set; }
        public short VY { get; set; }
        public byte MoveActionLast { get; set; }
        public short FHLast { get; set; }

        public MovementPath()
        {
            _path = new List<Movement>();
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<short>(X);
            packet.Encode<short>(Y);
            packet.Encode<short>(VX);
            packet.Encode<short>(VY);

            packet.Encode<byte>((byte) _path.Count);
            _path.ForEach(m =>
            {
                packet.Encode<byte>(m.Attr);

                switch (m.Attr)
                {
                    case 0x0:
                    case 0x5:
                    case 0xC:
                    case 0xE:
                    case 0x23:
                    case 0x24:
                        packet.Encode<short>(m.X);
                        packet.Encode<short>(m.Y);
                        packet.Encode<short>(m.VX);
                        packet.Encode<short>(m.VY);
                        packet.Encode<short>(m.FH);

                        if (m.Attr == 0xC)
                            packet.Encode<short>(m.FHFallStart);

                        packet.Encode<short>(m.XOffset);
                        packet.Encode<short>(m.YOffset);
                        goto default;
                    default:
                        packet.Encode<byte>(m.MoveAction);
                        packet.Encode<short>(m.Elapse);
                        break;
                    case 0x1:
                    case 0x2:
                    case 0xD:
                    case 0x10:
                    case 0x12:
                    case 0x1F:
                    case 0x20:
                    case 0x21:
                    case 0x22:
                        packet.Encode<short>(m.VX);
                        packet.Encode<short>(m.VY);
                        goto default;
                    case 0x14:
                    case 0x15:
                    case 0x16:
                    case 0x17:
                    case 0x18:
                    case 0x19:
                    case 0x1A:
                    case 0x1B:
                    case 0x1C:
                    case 0x1D:
                    case 0x1E:
                        goto default;
                    case 0x3:
                    case 0x4:
                    case 0x6:
                    case 0x7:
                    case 0x8:
                    case 0xA:
                        packet.Encode<short>(m.X);
                        packet.Encode<short>(m.Y);
                        packet.Encode<short>(m.FH);
                        goto default;
                    case 0xB:
                        packet.Encode<short>(m.VX);
                        packet.Encode<short>(m.VY);

                        packet.Encode<short>(m.FHFallStart);
                        goto default;
                    case 0x11:
                        packet.Encode<short>(m.X);
                        packet.Encode<short>(m.Y);
                        packet.Encode<short>(m.VX);
                        packet.Encode<short>(m.VY);
                        goto default;
                    case 0x9:
                        packet.Encode<bool>(m.Stat);
                        break;
                }
            });
        }

        public void Decode(IPacket packet)
        {
            X = packet.Decode<short>();
            Y = packet.Decode<short>();
            VX = packet.Decode<short>();
            VY = packet.Decode<short>();

            var size = packet.Decode<byte>();

            for (var i = 0; i < size; i++)
            {
                var m = new Movement();

                m.Attr = packet.Decode<byte>();

                switch (m.Attr)
                {
                    case 0x0:
                    case 0x5:
                    case 0xC:
                    case 0xE:
                    case 0x23:
                    case 0x24:
                        m.X = packet.Decode<short>();
                        m.Y = packet.Decode<short>();
                        m.VX = packet.Decode<short>();
                        m.VY = packet.Decode<short>();
                        m.FH = packet.Decode<short>();

                        FHLast = m.FH;

                        if (m.Attr == 0xC)
                            m.FHFallStart = packet.Decode<short>();

                        m.XOffset = packet.Decode<short>();
                        m.YOffset = packet.Decode<short>();
                        goto default;
                    default:
                        m.MoveAction = packet.Decode<byte>();
                        m.Elapse = packet.Decode<short>();

                        MoveActionLast = m.MoveAction;

                        X = m.X;
                        Y = m.Y;
                        VX = m.VX;
                        VY = m.VY;
                        break;
                    case 0x1:
                    case 0x2:
                    case 0xD:
                    case 0x10:
                    case 0x12:
                    case 0x1F:
                    case 0x20:
                    case 0x21:
                    case 0x22:
                        m.X = X;
                        m.Y = Y;
                        m.FH = 0;

                        m.VX = packet.Decode<short>();
                        m.VY = packet.Decode<short>();
                        goto default;
                    case 0x14:
                    case 0x15:
                    case 0x16:
                    case 0x17:
                    case 0x18:
                    case 0x19:
                    case 0x1A:
                    case 0x1B:
                    case 0x1C:
                    case 0x1D:
                    case 0x1E:
                        m.X = X;
                        m.Y = Y;
                        m.VX = VX;
                        m.VY = VY;
                        goto default;
                    case 0x3:
                    case 0x4:
                    case 0x6:
                    case 0x7:
                    case 0x8:
                    case 0xA:
                        m.X = packet.Decode<short>();
                        m.Y = packet.Decode<short>();
                        m.FH = packet.Decode<short>();

                        FHLast = m.FH;

                        m.VY = 0;
                        m.VX = 0;
                        goto default;
                    case 0xB:
                        m.X = X;
                        m.Y = Y;
                        m.FH = 0;

                        m.VX = packet.Decode<short>();
                        m.VY = packet.Decode<short>();

                        m.FHFallStart = packet.Decode<short>();
                        goto default;
                    case 0x11:
                        m.X = packet.Decode<short>();
                        m.Y = packet.Decode<short>();
                        m.VX = packet.Decode<short>();
                        m.VY = packet.Decode<short>();
                        goto default;
                    case 0x9:
                        m.Stat = packet.Decode<bool>();
                        m.VY = 0;
                        m.VX = 0;
                        m.X = X;
                        m.Y = Y;
                        m.Elapse = 0;
                        MoveActionLast = 0;
                        m.MoveAction = 0;
                        FHLast = 0;
                        m.FH = 0;
                        break;
                }

                _path.Add(m);
            }
        }
    }
}