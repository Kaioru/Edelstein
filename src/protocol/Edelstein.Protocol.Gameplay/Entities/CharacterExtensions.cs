using Edelstein.Protocol.Gameplay.Contracts.Packets.Shared;

namespace Edelstein.Protocol.Gameplay.Entities;

public static class CharacterExtensions
{
    public static StructuredCharacterStat ToStructuredCharacterStat(this Character character)
        => new()
        {
            ID = character.ID,
            Name = character.Name,
            Gender = character.Gender,
            Skin = character.Skin,
            Face = character.Face,
            Hair = character.Hair,

            Level = character.Level,
            Job = character.Job,
            STR = character.STR,
            DEX = character.DEX,
            INT = character.INT,
            LUK = character.LUK,
            HP = character.HP,
            MaxHP = character.MaxHP,
            MP = character.MP,
            MaxMP = character.MaxMP,

            AP = character.AP,
            SP = character.SP,

            EXP = character.EXP,
            POP = character.POP,
            TempEXP = character.TempEXP,

            PosMap = character.FieldID,
            Portal = character.FieldPortal,

            Playtime = character.PlayTime,

            SubJob = character.SubJob
        };

    public static StructuredAvatarLook ToStructuredAvatarLook(this Character character)
    {
        // TODO inventories
        
        return new StructuredAvatarLook
        {
            Gender = character.Gender,
            Skin = character.Skin,
            Face = character.Face,
            Hair = character.Hair
        };
    }
}
