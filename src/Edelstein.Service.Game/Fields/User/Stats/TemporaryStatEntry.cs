using System;
using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Fields.User.Stats
{
    public class TemporaryStatEntry
    {
        public TemporaryStatType Type { get; set; }
        public short Option { get; set; }
        public int TemplateID { get; set; }
        public DateTime? DateExpire { get; set; }

        public void Encode(IPacket packet)
        {
            packet.Encode<short>(Option);
            packet.Encode<int>(TemplateID);
            packet.Encode<int>(DateExpire.HasValue
                ? (int) (DateExpire.Value - DateTime.Now).TotalMilliseconds
                : int.MaxValue);
        }

        public void EncodeRemote(IPacket packet, int size)
        {
            switch (size)
            {
                case 1:
                    packet.Encode<byte>((byte) Option);
                    break;
                case 2:
                    packet.Encode<short>(Option);
                    break;
                case 4:
                    packet.Encode<int>(Option);
                    break;
            }
        }
    }
}