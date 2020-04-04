using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements
{
    public abstract class AbstractMoveFragment : IMoveFragment
    {
        public MoveFragmentAttribute Attribute { get; }

        protected AbstractMoveFragment(MoveFragmentAttribute attribute, IPacketDecoder packet)
        {
            Attribute = attribute;
            Decode(packet);
        }

        public abstract void Apply(IMoveContext context);

        public void Decode(IPacketDecoder packet)
            => DecodeData(packet);

        public void Encode(IPacketEncoder packet)
        {
            packet.EncodeByte((byte) Attribute);
            EncodeData(packet);
        }

        public abstract void DecodeData(IPacketDecoder packet);
        public abstract void EncodeData(IPacketEncoder packet);
    }
}