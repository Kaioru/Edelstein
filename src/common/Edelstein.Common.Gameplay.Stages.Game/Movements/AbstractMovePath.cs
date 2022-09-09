using Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements;

public abstract class AbstractMovePath<TMoveAction> : IMovePath<TMoveAction> where TMoveAction : IMoveAction
{
    private readonly ICollection<AbstractMovePathFragment<TMoveAction>> _fragments;
    private int _duration;
    private IPoint2D _position;
    private byte _state;
    private IPoint2D _vPosition;

    public AbstractMovePath() => _fragments = new List<AbstractMovePathFragment<TMoveAction>>();

    public byte? ActionRaw { get; set; }

    public TMoveAction? Action { get; set; }
    public IPoint2D? Position { get; set; }
    public int? Foothold { get; set; }

    public virtual void ReadFrom(IPacketReader reader)
    {
        _duration = reader.ReadInt();
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
                case MovePathFragmentType.DragDown:
                case MovePathFragmentType.Wings:
                case MovePathFragmentType.MobAttackLeap:
                case MovePathFragmentType.BattlePVPMugongSomersault:
                case MovePathFragmentType.BattlePVPHelenaStepshot:
                    _fragments.Add(reader.Read(new NormalPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.Jump:
                case MovePathFragmentType.Impact:
                case MovePathFragmentType.StartWings:
                case MovePathFragmentType.MobToss:
                case MovePathFragmentType.MobTossSlowdown:
                case MovePathFragmentType.DashSlide:
                case MovePathFragmentType.MobStopNodeStart:
                case MovePathFragmentType.MobBeforeNode:
                case MovePathFragmentType.MobTeleport:
                case MovePathFragmentType.MobAttackRush:
                    _fragments.Add(reader.Read(new JumpPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.Immediate:
                case MovePathFragmentType.Teleport:
                case MovePathFragmentType.RandomTeleport:
                case MovePathFragmentType.DemonTraceTeleport:
                case MovePathFragmentType.ReturnTeleport:
                case MovePathFragmentType.Assaulter:
                case MovePathFragmentType.Assassination:
                case MovePathFragmentType.Rush:
                case MovePathFragmentType.SitDown:
                case MovePathFragmentType.BlinkLight:
                case MovePathFragmentType.TeleportZero1:
                case MovePathFragmentType.ZeroTag:
                case MovePathFragmentType.RetreatShot:
                case MovePathFragmentType.DbBladeAscension:
                case MovePathFragmentType.MobRightAngle:
                case MovePathFragmentType.PinkbeanRollingAir:
                case MovePathFragmentType.FinalToss:
                case MovePathFragmentType.TeleportKinesis1:
                case MovePathFragmentType.TeleportAran1:
                case (MovePathFragmentType)82:
                    _fragments.Add(reader.Read(new TeleportPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.StatChange:
                    _fragments.Add(reader.Read(new StatChangePathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.StartFallDown:
                case MovePathFragmentType.StartDragDown:
                    _fragments.Add(reader.Read(new StartFallDownPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.FlyingBlock:
                    _fragments.Add(reader.Read(new FlyingBlockPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.DoubleJump:
                case MovePathFragmentType.DoubleJumpDown:
                case MovePathFragmentType.TripleJump:
                case MovePathFragmentType.FlashJumpChangeEff:
                case MovePathFragmentType.RocketBooster:
                case MovePathFragmentType.BackstepShot:
                case MovePathFragmentType.CannonJump:
                case MovePathFragmentType.QuickSilverJump:
                case MovePathFragmentType.MobPowerKnockback:
                case MovePathFragmentType.VerticalJump:
                case MovePathFragmentType.CustomImpact:
                case MovePathFragmentType.CustomImpact2:
                case MovePathFragmentType.CombatStep:
                case MovePathFragmentType.Hit:
                case MovePathFragmentType.TimeBombAttack:
                case MovePathFragmentType.SnowballTouch:
                case MovePathFragmentType.BuffZoneEffect:
                case MovePathFragmentType.LeafTornado:
                case MovePathFragmentType.StylishRope:
                case MovePathFragmentType.RopeConnect:
                case MovePathFragmentType.StrikerUppercut:
                case MovePathFragmentType.Crawl:
                case MovePathFragmentType.TeleportByMobSkillArea:
                case MovePathFragmentType.StarPlanetRidingBooster:
                case MovePathFragmentType.UserToss:
                case MovePathFragmentType.SlashJump:
                case MovePathFragmentType.MobLadder:
                case MovePathFragmentType.SunOfGlory:
                case MovePathFragmentType.HookshotStart:
                case MovePathFragmentType.Hookshot:
                case MovePathFragmentType.PinkBeanPogoStick:
                case MovePathFragmentType.NightlordShadowWeb:
                case MovePathFragmentType.RwExplosionCannon:
                case (MovePathFragmentType)83:
                    _fragments.Add(reader.Read(new ActionPathFragment<TMoveAction>(attribute)));
                    break;
                case MovePathFragmentType.AngleImpact:
                case MovePathFragmentType.MobAttackRushStop:
                case (MovePathFragmentType)85:
                    _fragments.Add(reader.Read(new AngledPathFragment<TMoveAction>(attribute)));
                    break;
            }
        }

        _state = reader.ReadByte();

        foreach (var fragment in _fragments)
            fragment.Apply(this);

        if (ActionRaw.HasValue)
            Action = GetActionFromRaw(ActionRaw.Value);
    }

    public virtual void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt(_duration);
        writer.WritePoint2D(_position);
        writer.WritePoint2D(_vPosition);

        writer.WriteByte((byte)_fragments.Count);
        foreach (var fragment in _fragments)
            writer.Write(fragment);

        writer.WriteByte(_state);
    }

    protected abstract TMoveAction GetActionFromRaw(byte raw);
}
