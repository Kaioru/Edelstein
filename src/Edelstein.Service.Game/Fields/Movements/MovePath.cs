using System.Collections.Generic;
using System.Drawing;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Movements.Fragments;
using Edelstein.Service.Game.Fields.Objects;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.Movements
{
    public class MovePath : IMoveFragment
    {
        private readonly ICollection<IMoveFragment> _fragments;

        public Point Position { get; set; }
        public Point VPosition { get; set; }

        public MovePath(IPacket packet)
        {
            _fragments = new List<IMoveFragment>();
            Decode(packet);
        }

        public void Apply(IFieldLife life)
        {
            _fragments.ForEach(f => f.Apply(life));
        }

        public void Decode(IPacket packet)
        {
            Position = packet.Decode<Point>();
            VPosition = packet.Decode<Point>();

            var size = packet.Decode<byte>();

            for (var i = 0; i < size; i++)
            {
                var attribute = (MovePathAttribute) packet.Decode<byte>();

                switch (attribute)
                {
                    case MovePathAttribute.Normal:
                    case MovePathAttribute.HangOnBack:
                    case MovePathAttribute.FallDown:
                    case MovePathAttribute.Wings:
                    case MovePathAttribute.MobAttackRush:
                    case MovePathAttribute.MobAttackRushStop:
                        _fragments.Add(new NormalMoveFragment(attribute, packet));
                        break;
                    case MovePathAttribute.Jump:
                    case MovePathAttribute.Impact:
                    case MovePathAttribute.StartWings:
                    case MovePathAttribute.MobToss:
                    case MovePathAttribute.DashSlide:
                    case MovePathAttribute.MobLadder:
                    case MovePathAttribute.MobRightAngle:
                    case MovePathAttribute.MobStopNodeStart:
                    case MovePathAttribute.MobBeforeNode:
                        _fragments.Add(new JumpMoveFragment(attribute, packet));
                        break;
                    case MovePathAttribute.FlashJump:
                    case MovePathAttribute.RocketBooster:
                    case MovePathAttribute.BackStepShot:
                    case MovePathAttribute.MobPowerKnockBack:
                    case MovePathAttribute.VerticalJump:
                    case MovePathAttribute.CustomImpact:
                    case MovePathAttribute.CombatStep:
                    case MovePathAttribute.Hit:
                    case MovePathAttribute.TimeBombAttack:
                    case MovePathAttribute.SnowballTouch:
                    case MovePathAttribute.BuffZoneEffect:
                        _fragments.Add(new ActionMoveFragment(attribute, packet));
                        break;
                    case MovePathAttribute.Immediate:
                    case MovePathAttribute.Teleport:
                    case MovePathAttribute.Assaulter:
                    case MovePathAttribute.Assassination:
                    case MovePathAttribute.Rush:
                    case MovePathAttribute.SitDown:
                        _fragments.Add(new TeleportMoveFragment(attribute, packet));
                        break;
                    case MovePathAttribute.StartFallDown:
                        _fragments.Add(new StartFallDownMoveFragment(attribute, packet));
                        break;
                    case MovePathAttribute.FlyingBlock:
                        _fragments.Add(new FlyingBlockMoveFragment(attribute, packet));
                        break;
                    case MovePathAttribute.StatChange:
                        _fragments.Add(new StatChangeMoveFragment(attribute, packet));
                        break;
                }
            }
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<Point>(Position);
            packet.Encode<Point>(VPosition);

            packet.Encode<byte>((byte) _fragments.Count);
            _fragments.ForEach(f => f.Encode(packet));
        }
    }
}