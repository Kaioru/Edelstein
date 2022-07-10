using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments
{
    public class StatChangeMoveFragment : AbstractMoveFragment
    {
        private bool _stat;

        public StatChangeMoveFragment(
            MoveFragmentAttribute attribute,
            IPacketReader reader
        ) : base(attribute, reader)
        {
        }

        protected override void ReadData(IPacketReader reader)
        {
            _stat = reader.ReadBool();
        }

        protected override void WriteData(IPacketWriter writer)
        {
            writer.WriteBool(_stat);
        }
    }
}
