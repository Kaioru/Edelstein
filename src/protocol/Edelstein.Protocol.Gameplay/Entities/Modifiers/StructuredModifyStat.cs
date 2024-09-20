using System.IO;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities.Modifiers;

public record StructuredModifyStat : StructuredBasePacket, IBinarySerializable
{
    public byte? Skin { get; set; }
    public int? Face { get; set; }
    public int? Hair { get; set; }

    // long Pet1 { get; set; }
    // long Pet2 { get; set; }
    // long Pet3 { get; set; }

    public byte? Level { get; set; }
    public short? Job { get; set; }

    public short? STR { get; set; }
    public short? DEX { get; set; }
    public short? INT { get; set; }
    public short? LUK { get; set; }

    public int? HP { get; set; }
    public int? MaxHP { get; set; }
    public int? MP { get; set; }
    public int? MaxMP { get; set; }

    public short? AP { get; set; }
    public short? SP { get; set; }

    public int? EXP { get; set; }
    public short? POP { get; set; }

    public int? Money { get; set; }
    public int? TempEXP { get; set; }
    
    public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var writer = new BinaryWriter(stream);
        var flags =
            (Skin != null ? ModifyStatType.Skin : 0) |
            (Face != null ? ModifyStatType.Face : 0) |
            (Hair != null ? ModifyStatType.Hair : 0) |
            (Level != null ? ModifyStatType.Level : 0) |
            (Job != null ? ModifyStatType.Job : 0) |
            (STR != null ? ModifyStatType.STR : 0) |
            (DEX != null ? ModifyStatType.DEX : 0) |
            (INT != null ? ModifyStatType.INT : 0) |
            (LUK != null ? ModifyStatType.LUK : 0) |
            (HP != null ? ModifyStatType.HP : 0) |
            (MaxHP != null ? ModifyStatType.MaxHP : 0) |
            (MP != null ? ModifyStatType.MP : 0) |
            (MaxMP != null ? ModifyStatType.MaxMP : 0) |
            (AP != null ? ModifyStatType.AP : 0) |
            (SP != null ? ModifyStatType.SP : 0) |
            (EXP != null ? ModifyStatType.EXP : 0) |
            (POP != null ? ModifyStatType.POP : 0) |
            (Money != null ? ModifyStatType.Money : 0) |
            (TempEXP != null ? ModifyStatType.TempEXP : 0);
        
        writer.Write((int)flags);
        
        if (Skin != null) writer.Write(Skin.Value);
        if (Face != null) writer.Write(Face.Value);
        if (Hair != null) writer.Write(Hair.Value);
        if (Level != null) writer.Write(Level.Value);
        if (Job != null) writer.Write(Job.Value);
        if (STR != null) writer.Write(STR.Value);
        if (DEX != null) writer.Write(DEX.Value);
        if (INT != null) writer.Write(INT.Value);
        if (LUK != null) writer.Write(LUK.Value);
        if (HP != null) writer.Write(HP.Value);
        if (MaxHP != null) writer.Write(MaxHP.Value);
        if (MP != null) writer.Write(MP.Value);
        if (MaxMP != null) writer.Write(MaxMP.Value);
        if (AP != null) writer.Write(AP.Value);
        if (SP != null) writer.Write(SP.Value);
        if (EXP != null) writer.Write(EXP.Value);
        if (POP != null) writer.Write(POP.Value);
        if (Money != null) writer.Write(Money.Value);
        if (TempEXP != null) writer.Write(TempEXP.Value);
    }
    
    public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var reader = new BinaryReader(stream);
        var flags = (ModifyStatType)reader.ReadInt32();

        if (flags.HasFlag(ModifyStatType.Skin)) Skin = reader.ReadByte();
        if (flags.HasFlag(ModifyStatType.Face)) Face = reader.ReadInt32();
        if (flags.HasFlag(ModifyStatType.Hair)) Hair = reader.ReadInt32();
        if (flags.HasFlag(ModifyStatType.Level)) Level = reader.ReadByte();
        if (flags.HasFlag(ModifyStatType.Job)) Job = reader.ReadInt16();
        if (flags.HasFlag(ModifyStatType.STR)) STR = reader.ReadInt16();
        if (flags.HasFlag(ModifyStatType.DEX)) DEX = reader.ReadInt16();
        if (flags.HasFlag(ModifyStatType.INT)) INT = reader.ReadInt16();
        if (flags.HasFlag(ModifyStatType.LUK)) LUK = reader.ReadInt16();
        if (flags.HasFlag(ModifyStatType.HP)) HP = reader.ReadInt32();
        if (flags.HasFlag(ModifyStatType.MaxHP)) MaxHP = reader.ReadInt32();
        if (flags.HasFlag(ModifyStatType.MP)) MP = reader.ReadInt32();
        if (flags.HasFlag(ModifyStatType.MaxMP)) MaxMP = reader.ReadInt32();
        if (flags.HasFlag(ModifyStatType.AP)) AP = reader.ReadInt16();
        if (flags.HasFlag(ModifyStatType.SP)) SP = reader.ReadInt16();
        if (flags.HasFlag(ModifyStatType.EXP)) EXP = reader.ReadInt32();
        if (flags.HasFlag(ModifyStatType.POP)) POP = reader.ReadInt16();
        if (flags.HasFlag(ModifyStatType.Money)) Money = reader.ReadInt32();
        if (flags.HasFlag(ModifyStatType.TempEXP)) TempEXP = reader.ReadInt32();
    }
}
