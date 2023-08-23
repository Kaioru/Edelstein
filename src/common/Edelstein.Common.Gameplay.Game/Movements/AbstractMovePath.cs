using Edelstein.Common.Gameplay.Game.Movements.Fragments;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Movements;

public abstract class AbstractMovePath<TMoveAction> : IMovePath<TMoveAction> where TMoveAction : IMoveAction
{
    private readonly ICollection<AbstractMovePathFragment<TMoveAction>> _fragments;
    private IPoint2D _position;
    private IPoint2D _vPosition;

    public byte? ActionRaw { get; set; }

    public TMoveAction? Action { get; set; }
    public IPoint2D? Position { get; set; }
    public int? FootholdID { get; set; }
    
    protected AbstractMovePath() => _fragments = new List<AbstractMovePathFragment<TMoveAction>>();

    public virtual void ReadFrom(IPacketReader reader)
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
                    _fragments.Add(reader.Read(new NormalPathFragment<TMoveAction>(attribute)));
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
                    _fragments.Add(reader.Read(new JumpPathFragment<TMoveAction>(attribute)));
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
                    _fragments.Add(reader.Read(new ActionPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.Immediate:
                case MovePathFragmentType.Teleport:
                case MovePathFragmentType.Assaulter:
                case MovePathFragmentType.Assassination:
                case MovePathFragmentType.Rush:
                case MovePathFragmentType.SitDown:
                    _fragments.Add(reader.Read(new TeleportPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.StartFallDown:
                    _fragments.Add(reader.Read(new StartFallDownPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.FlyingBlock:
                    _fragments.Add(reader.Read(new FlyingBlockPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.StatChange:
                    _fragments.Add(reader.Read(new StatChangePathFragment<TMoveAction>(attribute)));
                    break;
            }
        }

        foreach (var fragment in _fragments)
            fragment.Apply(this);

        if (ActionRaw.HasValue)
            Action = GetActionFromRaw(ActionRaw.Value);
    }

    public virtual void WriteTo(IPacketWriter writer)
    {
        writer.WritePoint2D(_position);
        writer.WritePoint2D(_vPosition);

        writer.WriteByte((byte)_fragments.Count);
        foreach (var fragment in _fragments)
            writer.Write(fragment);
    }

    protected abstract TMoveAction GetActionFromRaw(byte raw);
}
