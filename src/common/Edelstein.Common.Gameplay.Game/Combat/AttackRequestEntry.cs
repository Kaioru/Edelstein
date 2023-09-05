﻿using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Combat;

public class AttackRequestEntry : IAttackRequestEntry, IPacketReadable
{
    private readonly int _damagePerMob;
    
    public AttackRequestEntry(int damagePerMob)
    {
        _damagePerMob = damagePerMob;
        Damage = new int[_damagePerMob];
    }

    public int MobID { get; private set; }
    
    public byte HitAction { get; private set; }
    public byte ForeAction { get; private set; }
    
    public bool IsLeft { get; private set; }
    
    public byte FrameIDx { get; private set; }
    
    public byte CalcDamageStatIndex { get; private set; }
    public bool IsDoomed { get; private set; }
    
    public IPoint2D HitPosition { get; private set; }
    public IPoint2D PrevPosition { get; private set; }
    
    public short Delay { get; private set; }
    
    public int[] Damage { get; }
    
    public void ReadFrom(IPacketReader reader)
    {
        MobID = reader.ReadInt();
        HitAction = reader.ReadByte();

        var v33 = reader.ReadByte();
        ForeAction = (byte)(v33 & 0x7F);
        IsLeft = (v33 >> 7 & 1) > 0; // left

        FrameIDx = reader.ReadByte();

        var v34 = reader.ReadByte();
        CalcDamageStatIndex = (byte)(v34 & 0x7F);
        IsDoomed = (v34 >> 7 & 1) > 0;
            
        HitPosition = reader.ReadPoint2D();
        PrevPosition = reader.ReadPoint2D();

        Delay = reader.ReadShort();

        for (var i = 0; i < _damagePerMob; i++)
            Damage[i] = reader.ReadInt();

        _ = reader.ReadInt(); // mob crc
    }
}