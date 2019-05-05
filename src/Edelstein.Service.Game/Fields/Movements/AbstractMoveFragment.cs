using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements
{
    public abstract class AbstractMoveFragment : IMoveFragment
    {
        protected MovePathAttribute Attribute { get; set; }

        public AbstractMoveFragment(MovePathAttribute attribute, IPacket packet)
        {
            Attribute = attribute;
            Decode(packet);
        }

        public abstract void Apply(IFieldLife life);

        public void Decode(IPacket packet)
        {
            DecodeData(packet);
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) Attribute);
            EncodeData(packet);
        }

        public abstract void DecodeData(IPacket packet);
        public abstract void EncodeData(IPacket packet);
    }
}