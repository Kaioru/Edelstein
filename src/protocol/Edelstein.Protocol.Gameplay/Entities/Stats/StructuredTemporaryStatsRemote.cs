using System;
using System.IO;
using BinarySerialization;

namespace Edelstein.Protocol.Gameplay.Entities.Stats;

public class StructuredTemporaryStatsRemote : IBinarySerializable
{
    [Ignore] public required ITemporaryStats Stats { get; init; }
    
    public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        var now = DateTime.UtcNow;
        using var writer = new BinaryWriter(stream);
        
        writer.WriteTSFlag(Stats);

        foreach (var type in TemporaryStatOrder.WriteOrderRemote)
        {
            if (!Stats.Records.TryGetValue(type, out var stat)) continue;

            switch (type)
            {
                case TemporaryStatType.Speed:
                case TemporaryStatType.ComboCounter:
                case TemporaryStatType.Cyclone:
                    writer.Write((byte)stat.Option);
                    break;
                case TemporaryStatType.Poison:
                    writer.Write((short) stat.Option);
                    writer.Write(stat.Reason);
                    break;
                case TemporaryStatType.Morph:
                case TemporaryStatType.Ghost:
                    writer.Write((short)stat.Option);
                    break;
                case TemporaryStatType.SpiritJavelin: 
                case TemporaryStatType.RespectPImmune: 
                case TemporaryStatType.RespectMImmune: 
                case TemporaryStatType.DefenseAtt: 
                case TemporaryStatType.DefenseState: 
                case TemporaryStatType.MagicShield:
                    writer.Write(stat.Option);
                    break;
                default:
                    writer.Write(stat.Reason);
                    break;
            }
        }
        
        writer.Write(Stats[TemporaryStatType.DefenseAtt]?.Option ?? 0);
        writer.Write(Stats[TemporaryStatType.DefenseState]?.Option ?? 0);
        
        writer.WriteTSTwoState(Stats, now);
    }

    public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
    }
}
