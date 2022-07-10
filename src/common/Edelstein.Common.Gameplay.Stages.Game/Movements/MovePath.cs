using System.Collections.Generic;
using Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Utils;
using Edelstein.Protocol.Util.Spatial;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements
{
    public class MovePath : IMovePath, IPacketReadable, IPacketWritable
    {
        private readonly ICollection<AbstractMoveFragment> _fragments;
        private Point2D _position;
        private Point2D _vPosition;

        public MoveActionType? Action { get; set; }
        public Point2D? Position { get; set; }
        public short? FootholdID { get; set; }

        public MovePath()
        {
            _fragments = new List<AbstractMoveFragment>();
        }

        public void ReadFromPacket(IPacketReader reader)
        {
            _position = reader.ReadPoint2D();
            _vPosition = reader.ReadPoint2D();

            var size = reader.ReadByte();

            for (var i = 0; i < size; i++)
            {
                var attribute = (MoveFragmentAttribute)reader.ReadByte();

                switch (attribute)
                {
                    case MoveFragmentAttribute.Normal:
                    case MoveFragmentAttribute.HangOnBack:
                    case MoveFragmentAttribute.FallDown:
                    case MoveFragmentAttribute.Wings:
                    case MoveFragmentAttribute.MobAttackRush:
                    case MoveFragmentAttribute.MobAttackRushStop:
                        _fragments.Add(new NormalMoveFragment(attribute, reader));
                        break;
                    case MoveFragmentAttribute.Jump:
                    case MoveFragmentAttribute.Impact:
                    case MoveFragmentAttribute.StartWings:
                    case MoveFragmentAttribute.MobToss:
                    case MoveFragmentAttribute.DashSlide:
                    case MoveFragmentAttribute.MobLadder:
                    case MoveFragmentAttribute.MobRightAngle:
                    case MoveFragmentAttribute.MobStopNodeStart:
                    case MoveFragmentAttribute.MobBeforeNode:
                        _fragments.Add(new JumpMoveFragment(attribute, reader));
                        break;
                    case MoveFragmentAttribute.FlashJump:
                    case MoveFragmentAttribute.RocketBooster:
                    case MoveFragmentAttribute.BackStepShot:
                    case MoveFragmentAttribute.MobPowerKnockBack:
                    case MoveFragmentAttribute.VerticalJump:
                    case MoveFragmentAttribute.CustomImpact:
                    case MoveFragmentAttribute.CombatStep:
                    case MoveFragmentAttribute.Hit:
                    case MoveFragmentAttribute.TimeBombAttack:
                    case MoveFragmentAttribute.SnowballTouch:
                    case MoveFragmentAttribute.BuffZoneEffect:
                        _fragments.Add(new ActionMoveFragment(attribute, reader));
                        break;
                    case MoveFragmentAttribute.Immediate:
                    case MoveFragmentAttribute.Teleport:
                    case MoveFragmentAttribute.Assaulter:
                    case MoveFragmentAttribute.Assassination:
                    case MoveFragmentAttribute.Rush:
                    case MoveFragmentAttribute.SitDown:
                        _fragments.Add(new TeleportMoveFragment(attribute, reader));
                        break;
                    case MoveFragmentAttribute.StartFallDown:
                        _fragments.Add(new StartFallDownMoveFragment(attribute, reader));
                        break;
                    case MoveFragmentAttribute.FlyingBlock:
                        _fragments.Add(new FlyingBlockMoveFragment(attribute, reader));
                        break;
                    case MoveFragmentAttribute.StatChange:
                        _fragments.Add(new StatChangeMoveFragment(attribute, reader));
                        break;
                }
            }

            _fragments.ForEach(f => f.Apply(this));
        }

        public void WriteToPacket(IPacketWriter writer)
        {
            writer.WritePoint2D(_position);
            writer.WritePoint2D(_vPosition);

            writer.WriteByte((byte)_fragments.Count);
            _fragments.ForEach(f => writer.Write(f));
        }
    }
}
