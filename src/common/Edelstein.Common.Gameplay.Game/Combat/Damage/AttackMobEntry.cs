using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Combat.Damage;

public class AttackMobEntry : IAttackMobEntry, IPacketReadable
{
    private readonly byte _damagePerMob;
    private readonly bool _isSummonedAttack;

    public AttackMobEntry(byte damagePerMob, bool isSummonedAttack = false)
    {
        _damagePerMob = damagePerMob;
        _isSummonedAttack = isSummonedAttack;
    }

    public int MobID { get; private set; }
    
    public byte ActionHit { get; private set; }
    public byte ActionForeAndDir { get; private set; }
    public byte FrameIdx { get; private set; }
    
    public byte Option { get; private set; }
    
    public IPoint2D PositionHit { get; private set; }
    public IPoint2D PositionPrev { get; private set; }
    
    public short Delay { get; private set; }
    
    public int[] Damage { get; private set; }
    
    public void ReadFrom(IPacketReader reader)
    {
        MobID = reader.ReadInt();

        if (_isSummonedAttack) reader.ReadInt();

        ActionHit = reader.ReadByte();
        ActionForeAndDir = reader.ReadByte();
        FrameIdx = reader.ReadByte();

        Option = reader.ReadByte();

        PositionHit = reader.ReadPoint2D();
        PositionPrev = reader.ReadPoint2D();

        Delay = reader.ReadShort();

        Damage = new int[_damagePerMob];
        for (var i = 0; i < _damagePerMob; i++)
            Damage[i] = reader.ReadInt();

        if (!_isSummonedAttack)
            _ = reader.ReadInt();
    }
}
