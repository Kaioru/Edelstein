using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record MobMove() : StructuredSendPacket((short)PacketSendOperation.MobMove)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public bool NotForceLandingWhenDiscard { get; init; }
    [FieldOrder(2)] public bool NotChangeAction { get; init; }
    
    [FieldOrder(3)] 
    [FieldBitLength(4)]
    public byte MobCtrlState { get; init; }
    
    [FieldOrder(4)] 
    [FieldBitLength(1)]
    public bool RiseByToss { get; init; }
    
    [FieldOrder(5)]
    [FieldBitLength(1)]
    public bool RushMove { get; init; }
    
    [FieldOrder(6)]
    [FieldBitLength(2)]
    public bool DirLeft { get; init; }
    
    [FieldOrder(7)] public byte Action { get; init; }

    [FieldOrder(8)] public MobMoveInfoTarget TargetInfo { get; init; } = new MobMoveInfoTargetAttack();
    
    [FieldOrder(9)]
    public int MultiTargetForBallCount { get; init; }
    
    [FieldOrder(10)] 
    [FieldCount(nameof(MultiTargetForBallCount))]
    public List<MobMoveInfoMultiTargetForBall> MultiTargetForBall { get; init; } = new();
    
    [FieldOrder(11)]
    public int RandTimeForAreaAttackCount { get; init; }

    [FieldOrder(12)] 
    [FieldCount(nameof(RandTimeForAreaAttackCount))]
    public List<MobMoveInfoRandTimeForAreaAttack> RandTimeForAreaAttack { get; init; } = new();
    
    [FieldOrder(13)] public required StructuredMovePath Path { get; init; }
}

public record MobMoveInfoTarget : StructuredBasePacket;

public record MobMoveInfoTargetSkill : MobMoveInfoTarget
{
    [FieldOrder(0)] public byte SkillCommand { get; init; }
    [FieldOrder(1)] public byte SLV { get; init; }
    [FieldOrder(2)] public byte Option { get; init; }
}

public record MobMoveInfoTargetAttack : MobMoveInfoTarget
{
    [FieldOrder(0)] public int Info { get; init; }
}
