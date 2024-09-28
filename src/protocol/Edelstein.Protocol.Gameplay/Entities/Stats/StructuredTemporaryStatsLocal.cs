using System;
using System.IO;
using BinarySerialization;

namespace Edelstein.Protocol.Gameplay.Entities.Stats;

public record StructuredTemporaryStatsLocal : IBinarySerializable
{
    [Ignore] public required ITemporaryStats Stats { get; init; }
    
    public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        var now = DateTime.UtcNow;
        using var writer = new BinaryWriter(stream);
        
        writer.WriteTSFlag(Stats);
        
        foreach (var type in TemporaryStatOrder.WriteOrderLocal)
        {
            if (!Stats.Records.TryGetValue(type, out var stat)) continue;
            
            var time = (int)(stat.DateExpire.HasValue
                ? (stat.DateExpire.Value - now).TotalMilliseconds 
                : int.MaxValue);
            
            writer.Write((short)stat.Option);
            writer.Write(stat.Reason);
            writer.Write(time);
        }

        writer.Write(Stats[TemporaryStatType.DefenseAtt]?.Option ?? 0);
        writer.Write(Stats[TemporaryStatType.DefenseState]?.Option ?? 0);
        
        foreach (var type in TemporaryStatOrder.WriteOrderLocalSwallow)
        {
            if (!Stats.Records.TryGetValue(type, out var stat)) continue;
            
            var time = (int)(stat.DateExpire.HasValue
                ? (stat.DateExpire.Value - now).TotalMilliseconds 
                : int.MaxValue);

            writer.Write((byte)(time / 1000));
            break;
        }

        if (Stats[TemporaryStatType.Dice] != null)
        {
            writer.Write(Stats.DiceInfo?.MHPr ?? 0);
            writer.Write(Stats.DiceInfo?.MMPr ?? 0);
            writer.Write(Stats.DiceInfo?.Cr ?? 0);
            writer.Write(Stats.DiceInfo?.CDMin ?? 0);
            writer.Write(Stats.DiceInfo?.EVAr ?? 0);
            writer.Write(Stats.DiceInfo?.Ar ?? 0);
            writer.Write(Stats.DiceInfo?.Er ?? 0);
            writer.Write(Stats.DiceInfo?.PDDr ?? 0);
            writer.Write(Stats.DiceInfo?.MDDr ?? 0);
            writer.Write(Stats.DiceInfo?.PDr ?? 0);
            writer.Write(Stats.DiceInfo?.MDr ?? 0);
            writer.Write(Stats.DiceInfo?.DIPr ?? 0);
            writer.Write(Stats.DiceInfo?.PDamr ?? 0);
            writer.Write(Stats.DiceInfo?.MDamr ?? 0);
            writer.Write(Stats.DiceInfo?.PADr ?? 0);
            writer.Write(Stats.DiceInfo?.MADr ?? 0);
            writer.Write(Stats.DiceInfo?.EXPr ?? 0);
            writer.Write(Stats.DiceInfo?.IMPr ?? 0);
            writer.Write(Stats.DiceInfo?.ASRr ?? 0);
            writer.Write(Stats.DiceInfo?.TERr ?? 0);
            writer.Write(Stats.DiceInfo?.MESOr ?? 0);
        }
        
        if (Stats[TemporaryStatType.BlessingArmor] != null)
            writer.Write(Stats[TemporaryStatType.BlessingArmor]?.Option ?? 0);
        
        writer.WriteTSTwoState(Stats, now);
    }

    public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
    }
}
