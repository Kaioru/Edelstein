using Edelstein.Common.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob;

public class FieldMobMovePath : AbstractMovePath<IFieldMobMoveAction>, IFieldMobMovePath
{
    private byte v7 { get; set; }
    private byte centerSplit { get; set; }
    private byte v8 { get; set; }
    private readonly ICollection<long> _multiTargetForBall = new List<long>();
    private readonly ICollection<int> _randTimeForAreaAttack = new List<int>();

    public short MobCtrlSN { get; private set; }
    
    public bool NextAttackPossible => (v7 & 0xF) != 0;

    public int Action => centerSplit >> 1;
    public int Data { get; private set; }

    public override void ReadFrom(IPacketReader reader)
    {
        MobCtrlSN = reader.ReadShort();

        v7 = reader.ReadByte();

        centerSplit = reader.ReadByte();
        Data = reader.ReadInt();
        
        v8 = reader.ReadByte();
        
        var multiTargetForBall = reader.ReadInt();
        for (var i = 0; i < multiTargetForBall; i++) 
            _multiTargetForBall.Add(reader.ReadLong()); // int, int

        var randTimeForAreaAttack = reader.ReadInt();
        for (var i = 0; i < randTimeForAreaAttack; i++) 
            _randTimeForAreaAttack.Add(reader.ReadInt());
        
        reader.ReadInt(); // HackedCode
        reader.ReadInt(); // idk
        reader.ReadInt(); // HackedCodeCrc
        reader.ReadInt(); // idk

        base.ReadFrom(reader);
    }

    public override void WriteTo(IPacketWriter writer)
    {
        writer.WriteBool(false); // NotForceLandingWhenDiscard
        writer.WriteBool(false); // NotChangeAction
        writer.WriteBool(NextAttackPossible);
        writer.WriteByte(centerSplit);
        writer.WriteInt(Data);

        writer.WriteInt(_multiTargetForBall.Count);
        foreach (var l in _multiTargetForBall)
            writer.WriteLong(l);

        writer.WriteInt(_randTimeForAreaAttack.Count);
        foreach (var i in _randTimeForAreaAttack)
            writer.WriteInt(i);
        
        base.WriteTo(writer);
    }

    protected override IFieldMobMoveAction GetActionFromRaw(byte raw) => new FieldMobMoveAction(raw);
}
