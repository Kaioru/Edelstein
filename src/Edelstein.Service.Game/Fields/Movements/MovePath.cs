using System.Collections.Generic;
using System.Drawing;
using Edelstein.Core.Network.Packets;
using Edelstein.Service.Game.Fields.Movements.Fragments;
using Edelstein.Service.Game.Fields.Objects;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.Movements
{
    public class MovePath : IMovePath
    {
        private readonly ICollection<IMoveFragment> _fragments;
        private Point _position;
        private Point _vPosition;

        public MovePath(IPacketDecoder packet)
        {
            _fragments = new List<IMoveFragment>();
            Decode(packet);
        }

        public IMoveContext Apply(IFieldLife life)
        {
            var context = new MoveContext();

            _fragments.ForEach(f => f.Apply(context));

            if (context.MoveAction.HasValue) life.MoveAction = context.MoveAction.Value;
            if (context.Foothold.HasValue) life.Foothold = context.Foothold.Value;
            if (context.Position.HasValue) life.Position = context.Position.Value;

            return context;
        }

        public void Decode(IPacketDecoder packet)
        {
            _position = packet.DecodePoint();
            _vPosition = packet.DecodePoint();

            var size = packet.DecodeByte();

            for (var i = 0; i < size; i++)
            {
                var attribute = (MoveFragmentAttribute) packet.DecodeByte();

                switch (attribute)
                {
                    case MoveFragmentAttribute.Normal:
                    case MoveFragmentAttribute.HangOnBack:
                    case MoveFragmentAttribute.FallDown:
                    case MoveFragmentAttribute.Wings:
                    case MoveFragmentAttribute.MobAttackRush:
                    case MoveFragmentAttribute.MobAttackRushStop:
                        _fragments.Add(new NormalMoveFragment(attribute, packet));
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
                        _fragments.Add(new JumpMoveFragment(attribute, packet));
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
                        _fragments.Add(new ActionMoveFragment(attribute, packet));
                        break;
                    case MoveFragmentAttribute.Immediate:
                    case MoveFragmentAttribute.Teleport:
                    case MoveFragmentAttribute.Assaulter:
                    case MoveFragmentAttribute.Assassination:
                    case MoveFragmentAttribute.Rush:
                    case MoveFragmentAttribute.SitDown:
                        _fragments.Add(new TeleportMoveFragment(attribute, packet));
                        break;
                    case MoveFragmentAttribute.StartFallDown:
                        _fragments.Add(new StartFallDownMoveFragment(attribute, packet));
                        break;
                    case MoveFragmentAttribute.FlyingBlock:
                        _fragments.Add(new FlyingBlockMoveFragment(attribute, packet));
                        break;
                    case MoveFragmentAttribute.StatChange:
                        _fragments.Add(new StatChangeMoveFragment(attribute, packet));
                        break;
                }
            }
        }

        public void Encode(IPacketEncoder packet)
        {
            packet.EncodePoint(_position);
            packet.EncodePoint(_vPosition);

            packet.EncodeByte((byte) _fragments.Count);
            _fragments.ForEach(f => f.Encode(packet));
        }
    }
}