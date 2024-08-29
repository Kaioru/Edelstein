using System;
using System.Collections.Generic;
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

    public static StructuredCharacterLook ToStructuredCharacterLook(this Character character)
    {
        var inventory = character.Inventories[ItemInventoryType.Equip]?.Items ?? new Dictionary<short, ItemSlotBase>();
        var unseen = new Dictionary<byte, int>();
        var equip = new Dictionary<byte, int>();
        var weaponStickerID = 0;
        
        // TODO: evan gloves 1082262
        
        foreach (var kv in inventory.Where(kv => kv.Key < -100))
        {
            var slot = (byte)(Math.Abs(kv.Key) - 100);
            
            if (slot == (int)BodyPart.Weapon) 
                weaponStickerID = kv.Value.TemplateID;
            equip[slot] = kv.Value.TemplateID;
        }
        
        foreach (var kv in inventory.Where(kv => kv.Key is < 0 and > -100))
        {
            var slot = (byte)Math.Abs(kv.Key);
            
            if (!equip.ContainsKey(slot)) 
                equip[slot] = kv.Value.TemplateID;
            else 
                unseen[slot] = kv.Value.TemplateID;
        }
        
        return new StructuredCharacterLook
        {
            Gender = character.Gender,
            Skin = character.Skin,
            Face = character.Face,
            Hair = character.Hair,
            HairEquip = equip
                .Select(kv => new StructuredCharacterLookEquip
                {
                    BodyPart = kv.Key,
                    ItemID = kv.Value
                })
                .ToList(),
            UnseenEquip = unseen
                .Select(kv => new StructuredCharacterLookEquip
                {
                    BodyPart = kv.Key,
                    ItemID = kv.Value
                })
                .ToList(),
            WeaponStickerID = weaponStickerID
        };
    }
}
