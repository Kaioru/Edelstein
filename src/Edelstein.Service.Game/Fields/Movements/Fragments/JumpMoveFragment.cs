using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class JumpMoveFragment : ActionMoveFragment
    {
        public Point VPosition { get; set; }

        public JumpMoveFragment(MovePathAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void DecodeData(IPacket packet)
        {
            VPosition = packet.Decode<Point>();
            
            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<Point>(VPosition);
            
            base.EncodeData(packet);
        }
    }
}