﻿using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Common.Gameplay.Models.Characters;

public class CharacterWildHunterInfo : ICharacterWildHunterInfo
{
    public byte RidingType { get; set; }
    public int[] CaptureMob { get; } = new int[5];
}