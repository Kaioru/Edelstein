using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class ActionMoveFragment : AbstractMoveFragment
    {
        public byte MoveAction { get; set; }
        public short Elapse { get; set; }

        public ActionMoveFragment(MovePathAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void Apply(IFieldLife life)
        {
            life.MoveAction = MoveAction;
        }

        public override void DecodeData(IPacket packet)
        {
            MoveAction = packet.Decode<byte>();
            Elapse = packet.Decode<short>();
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<byte>(MoveAction);
            packet.Encode<short>(Elapse);
        }
    }
}