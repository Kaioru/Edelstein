using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Combat;

public class AttackRequest : IAttackRequest, IPacketReadable
{
    public AttackRequest(AttackType type) => Type = type;

    public AttackType Type { get; }
    
    public int DamagePerMob { get; private set; }
    public int MobCount { get; private set; }
    
    public int SkillID { get; private set; }
    public int Keydown { get; private set; }
    
    public bool IsFinalAfterSlashBlast { get; private set; }
    public bool IsSoulArrow { get; private set; }
    public bool IsShadowPartner { get; private set; }
    public bool IsSerialAttack { get; private set; }
    public bool IsSpiritJavelin { get; private set; }
    
    public bool IsLeft { get; private set; }
    
    public int Action { get; private set; }
    
    public byte AttackType { get; private set; }
    
    public int PartyCount { get; private set; }
    public int SpeedDegree { get; private set; }
    
    public int AttackTime { get; private set; }
    
    public int Phase { get; private set; }

    public ICollection<IAttackRequestEntry> Entries { get; } = new List<IAttackRequestEntry>();

    public void ReadFrom(IPacketReader reader)
    {
        _ = reader.ReadByte(); // bCurFieldKey
        _ = reader.ReadInt(); // dr0
        _ = reader.ReadInt(); // dr1

        var v6 = reader.ReadByte();
        DamagePerMob = v6 >> 0 & 0xF;
        MobCount = v6 >> 4 & 0xF;
        
        _ = reader.ReadInt(); // dr2
        _ = reader.ReadInt(); // dr3

        SkillID = reader.ReadInt();
        _ = reader.ReadBool(); // unk

        _ = reader.ReadInt(); // dr rand
        _ = reader.ReadInt(); // crc

        _ = reader.ReadInt(); // skillLevel crc
        _ = reader.ReadInt(); // skillLevel crc

        Keydown = 0;
        
        var option = reader.ReadByte();
        IsFinalAfterSlashBlast = (option & 0x0) > 0;
        IsSoulArrow = (option & 0x2) > 0;
        IsShadowPartner = (option & 0x8) > 0;
        //
        IsSerialAttack = (option & 0x20) > 0;
        IsSpiritJavelin = (option & 0x40) > 0;

        var v15 = reader.ReadShort();
        IsLeft = (v15 >> 15 & 1) > 0;
        Action = v15 & 0x7FFF;
        _ = reader.ReadInt(); // action crc?

        AttackType = reader.ReadByte();
        
        var v17 = reader.ReadByte();
        PartyCount = v17 >> 4;
        SpeedDegree = v17 & 0xF;
        
        AttackTime = reader.ReadInt();

        Phase = reader.ReadInt();
        
        for (var i = 0; i < MobCount; i++)
            Entries.Add(reader.Read(new AttackRequestEntry(DamagePerMob)));

        _ = reader.ReadPoint2D();
    }
}
