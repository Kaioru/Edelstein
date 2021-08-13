using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments
{
    public class ActionMoveFragment : AbstractMoveFragment
    {
        private MoveActionType _moveAction;
        private short _elapse;

        public ActionMoveFragment(
            MoveFragmentAttribute attribute,
            IPacketReader reader
        ) : base(attribute, reader)
        {
        }

        public override void Apply(MovePath path)
        {
            path.Action = _moveAction;
        }

        protected override void ReadData(IPacketReader reader)
        {
            _moveAction = (MoveActionType)reader.ReadByte();
            _elapse = reader.ReadShort();
        }

        protected override void WriteData(IPacketWriter writer)
        {
            writer.WriteByte((byte)_moveAction);
            writer.WriteShort(_elapse);
        }
    }
}
