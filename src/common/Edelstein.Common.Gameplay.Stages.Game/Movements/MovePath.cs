using Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements;

public class MovePath : IMovePath
{
    private readonly ICollection<AbstractMovePathFragment> _fragments;
    private IPoint2D _position;
    private IPoint2D _vPosition;

    public MovePath() => _fragments = new List<AbstractMovePathFragment>();

    public IPoint2D? Position { get; set; }
    public int? FootholdID { get; set; }

    public void ReadFrom(IPacketReader reader)
    {
        _position = reader.ReadPoint2D();
        _vPosition = reader.ReadPoint2D();

        var size = reader.ReadByte();

        for (var i = 0; i < size; i++)
        {
            var attribute = (MovePathFragmentType)reader.ReadByte();

            switch (attribute)
            {
                case MovePathFragmentType.Normal:
                case MovePathFragmentType.HangOnBack:
                case MovePathFragmentType.FallDown:
                case MovePathFragmentType.Wings:
                case MovePathFragmentType.MobAttackRush:
                case MovePathFragmentType.MobAttackRushStop:
                    _fragments.Add(reader.Read(new NormalPathFragment(attribute)));
                    break;
                case MovePathFragmentType.Jump:
                case MovePathFragmentType.Impact:
                case MovePathFragmentType.StartWings:
                case MovePathFragmentType.MobToss:
                case MovePathFragmentType.DashSlide:
                case MovePathFragmentType.MobLadder:
                case MovePathFragmentType.MobRightAngle:
                case MovePathFragmentType.MobStopNodeStart:
                case MovePathFragmentType.MobBeforeNode:
                    _fragments.Add(reader.Read(new JumpPathFragment(attribute)));
                    break;
                case MovePathFragmentType.FlashJump:
                case MovePathFragmentType.RocketBooster:
                case MovePathFragmentType.BackStepShot:
                case MovePathFragmentType.MobPowerKnockBack:
                case MovePathFragmentType.VerticalJump:
                case MovePathFragmentType.CustomImpact:
                case MovePathFragmentType.CombatStep:
                case MovePathFragmentType.Hit:
                case MovePathFragmentType.TimeBombAttack:
                case MovePathFragmentType.SnowballTouch:
                case MovePathFragmentType.BuffZoneEffect:
                    _fragments.Add(reader.Read(new ActionPathFragment(attribute)));
                    break;
                case MovePathFragmentType.Immediate:
                case MovePathFragmentType.Teleport:
                case MovePathFragmentType.Assaulter:
                case MovePathFragmentType.Assassination:
                case MovePathFragmentType.Rush:
                case MovePathFragmentType.SitDown:
                    _fragments.Add(reader.Read(new TeleportPathFragment(attribute)));
                    break;
                case MovePathFragmentType.StartFallDown:
                    _fragments.Add(reader.Read(new StartFallDownPathFragment(attribute)));
                    break;
                case MovePathFragmentType.FlyingBlock:
                    _fragments.Add(reader.Read(new FlyingBlockPathFragment(attribute)));
                    break;
                case MovePathFragmentType.StatChange:
                    _fragments.Add(reader.Read(new StatChangePathFragment(attribute)));
                    break;
            }
        }

        foreach (var fragment in _fragments)
            fragment.Apply(this);
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WritePoint2D(_position);
        writer.WritePoint2D(_vPosition);

        writer.WriteByte((byte)_fragments.Count);
        foreach (var fragment in _fragments)
            writer.Write(fragment);
    }
}
