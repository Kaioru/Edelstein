using System.IO;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Movements;

public record StructuredMovePathFragment : StructuredBasePacket, IBinarySerializable
{
    [Ignore] public required MovePathFragmentType Type { get; set; }
    
    [Ignore] public byte? Action { get; set; }
    [Ignore] public short? Offset { get; set; }
    
    [Ignore] public short? X { get; set; }
    [Ignore] public short? Y { get; set; }
    
    [Ignore] public short? VX { get; set; }
    [Ignore] public short? VY { get; set; }
    
    [Ignore] public short? Fh { get; set; }
    [Ignore] public short? FhFallStart { get; set; }
    
    [Ignore] public short? XOffset { get; set; }
    [Ignore] public short? YOffset { get; set; }
    
    [Ignore] public byte? Stat { get; set; }
    
    public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var writer = new BinaryWriter(stream);
        
        writer.Write((byte)Type);

        switch (Type)
        {
            case MovePathFragmentType.Normal:
            case MovePathFragmentType.HangOnBack:
            case MovePathFragmentType.FallDown:
            case MovePathFragmentType.Wings:
            case MovePathFragmentType.MobAttackRush:
            case MovePathFragmentType.MobAttackRushStop:
                writer.Write(X ?? 0);
                writer.Write(Y ?? 0);
                writer.Write(VX ?? 0);
                writer.Write(VY ?? 0);
                writer.Write(Fh ?? 0);
                
                if (Type == MovePathFragmentType.FallDown)
                    writer.Write(FhFallStart ?? 0);
                
                writer.Write(XOffset ?? 0);
                writer.Write(YOffset ?? 0);
                goto default;
            case MovePathFragmentType.Jump:
            case MovePathFragmentType.Impact:
            case MovePathFragmentType.StartWings:
            case MovePathFragmentType.MobToss:
            case MovePathFragmentType.DashSlide:
            case MovePathFragmentType.MobLadder:
            case MovePathFragmentType.MobRightAngle:
            case MovePathFragmentType.MobStopNodeStart:
            case MovePathFragmentType.MobBeforeNode:
                writer.Write(VX ?? 0);
                writer.Write(VY ?? 0);
                goto default;
            case MovePathFragmentType.Immediate:
            case MovePathFragmentType.Teleport:
            case MovePathFragmentType.Assaulter:
            case MovePathFragmentType.Assassination:
            case MovePathFragmentType.Rush:
            case MovePathFragmentType.SitDown:
                writer.Write(X ?? 0);
                writer.Write(Y ?? 0);
                writer.Write(Fh ?? 0);
                goto default;
            case MovePathFragmentType.StatChange:
                writer.Write(Stat ?? 0);
                break;
            case MovePathFragmentType.StartFallDown:
                writer.Write(VX ?? 0);
                writer.Write(VY ?? 0);                   
                writer.Write(FhFallStart ?? 0);
                goto default;
            case MovePathFragmentType.FlyingBlock:
                writer.Write(X ?? 0);
                writer.Write(Y ?? 0);
                writer.Write(VX ?? 0);
                writer.Write(VY ?? 0);
                break;
            case MovePathFragmentType.AranAdjust:
            case MovePathFragmentType.BmageAdjust:
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
            default:
                writer.Write(Action ?? 0);
                writer.Write(Offset ?? 0);
                break;
        }
    }

    public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
    {
        using var reader = new BinaryReader(stream);
        
        Type = (MovePathFragmentType)reader.ReadByte();

        switch (Type)
        {
            case MovePathFragmentType.Normal:
            case MovePathFragmentType.HangOnBack:
            case MovePathFragmentType.FallDown:
            case MovePathFragmentType.Wings:
            case MovePathFragmentType.MobAttackRush:
            case MovePathFragmentType.MobAttackRushStop:
                X = reader.ReadInt16();
                Y = reader.ReadInt16();
                VX = reader.ReadInt16();
                VY = reader.ReadInt16();
                Fh = reader.ReadInt16();

                if (Type == MovePathFragmentType.FallDown)
                    FhFallStart = reader.ReadInt16();
                
                XOffset = reader.ReadInt16();
                YOffset = reader.ReadInt16();
                goto default;
            case MovePathFragmentType.Jump:
            case MovePathFragmentType.Impact:
            case MovePathFragmentType.StartWings:
            case MovePathFragmentType.MobToss:
            case MovePathFragmentType.DashSlide:
            case MovePathFragmentType.MobLadder:
            case MovePathFragmentType.MobRightAngle:
            case MovePathFragmentType.MobStopNodeStart:
            case MovePathFragmentType.MobBeforeNode:
                VX = reader.ReadInt16();
                VY = reader.ReadInt16();
                goto default;
            case MovePathFragmentType.Immediate:
            case MovePathFragmentType.Teleport:
            case MovePathFragmentType.Assaulter:
            case MovePathFragmentType.Assassination:
            case MovePathFragmentType.Rush:
            case MovePathFragmentType.SitDown:
                X = reader.ReadInt16();
                Y = reader.ReadInt16();
                Fh = reader.ReadInt16();
                goto default;
            case MovePathFragmentType.StatChange:
                Stat = reader.ReadByte();
                break;
            case MovePathFragmentType.StartFallDown:
                VX = reader.ReadInt16();
                VY = reader.ReadInt16();     
                FhFallStart = reader.ReadInt16();
                goto default;
            case MovePathFragmentType.FlyingBlock:
                X = reader.ReadInt16();
                Y = reader.ReadInt16();
                VX = reader.ReadInt16();
                VY = reader.ReadInt16();
                break;
            case MovePathFragmentType.AranAdjust:
            case MovePathFragmentType.BmageAdjust:
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
            default:
                Action = reader.ReadByte();
                Offset = reader.ReadInt16();
                break;
        }
    }
}
