using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Protocol.Gameplay.Entities.Inventories;

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

    public static StructuredCharacterLook ToStructuredAvatarLook(this Character character)
    {
        var inventory = character.Inventories[ItemInventoryType.Equip]?.Items ?? ImmutableDictionary<short, ItemSlotBase>.Empty;
        var unseen = new int[60];
        var equip = new int[60];
        
        // TODO: evan gloves 1082262
        
        foreach (var kv in inventory.Where(kv => kv.Key < -100))
        {
            var id = Math.Abs(kv.Key) - 100;
            if (id == (int)BodyPart.Weapon) continue;
            equip[id] = kv.Value.TemplateID;
        }
        
        foreach (var kv in inventory.Where(kv => kv.Key is < 0 and > -100))
        {
            var id = Math.Abs(kv.Key);
            if (equip[id] == 0) equip[id] = kv.Value.TemplateID;
            else unseen[id] = kv.Value.TemplateID;
        }
        
        return new StructuredCharacterLook
        {
            Gender = character.Gender,
            Skin = character.Skin,
            Face = character.Face,
            Hair = character.Hair,
            HairEquip = equip
                .Where((_, v) => v != 0)
                .Select((k, v) => new StructuredCharacterLookEquip
                {
                    BodyPart = (byte)k,
                    ItemID = v
                })
                .ToList(),
            UnseenEquip = unseen
                .Where((_, v) => v != 0)
                .Select((k, v) => new StructuredCharacterLookEquip
                {
                    BodyPart = (byte)k,
                    ItemID = v
                })
                .ToList(),
            WeaponStickerID = inventory.TryGetValue(-((int)BodyPart.Weapon + 100), out var weaponSticker) 
                ? weaponSticker.TemplateID
                : 0
        };
    }
}
