using System;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public static class ItemSlotBaseExtensions
{
    public static StructuredItemSlot ToStructured(this ItemSlotBase item)
        => new()
        {
            Type = item switch
            {
                ItemSlotEquip => ItemSlotType.Equip,
                ItemSlotBundle => ItemSlotType.Bundle,
                ItemSlotPet => ItemSlotType.Pet,
                _ => throw new ArgumentOutOfRangeException(nameof(item))
            },
            Info = item switch
            {
                ItemSlotEquip equip => new StructuredItemSlotInfoEquip
                {
                    ItemID = equip.TemplateID,
                    
                    RUC = equip.RUC,
                    CUC = equip.CUC,
                    
                    STR = equip.STR,
                    DEX = equip.DEX,
                    INT = equip.INT,
                    LUK = equip.LUK,
                    MaxHP = equip.MaxHP,
                    MaxMP = equip.MaxMP,
                    PAD = equip.PAD,
                    MAD = equip.MAD,
                    PDD = equip.PDD,
                    MDD = equip.MDD,
                    ACC = equip.ACC,
                    EVA = equip.EVA,
                    
                    Craft = equip.Craft,
                    Speed = equip.Speed,
                    Jump = equip.Jump,
                    
                    Title = new LPString(equip.Title ?? ""),
                    Attribute = equip.Attribute,
                    LevelUpType = equip.LevelUpType,
                    Level = equip.Level,
                    EXP = equip.EXP,
                    Durability = equip.Durability ?? -1,
                    
                    IUC = equip.IUC,
                    
                    Grade = equip.Grade,
                    CHUC = equip.CHUC,
                    
                    Option1 = equip.Option1,
                    Option2 = equip.Option2,
                    Option3 = equip.Option3,
                    Socket1 = equip.Socket1,
                    Socket2 = equip.Socket2
                },
                ItemSlotBundle bundle => new StructuredItemSlotInfoBundle
                {
                    ItemID = bundle.TemplateID,
                    
                    Number = bundle.Number,
                    Title = new LPString(bundle.Title ?? ""),
                    Attribute = bundle.Attribute
                },
                ItemSlotPet pet => new StructuredItemSlotInfoPet
                {
                    ItemID = pet.TemplateID,
                    
                    PetName = pet.PetName,
                    PetAttribute = pet.PetAttribute,
                    PetSkill = pet.PetSkill,
                    
                    Level = pet.Level,
                    Tameness = pet.Tameness,
                    Repleteness = pet.Repleteness,
                    
                    DateDead = pet.DateDead.HasValue ? new FDateTime(pet.DateDead.Value) : new FDateTime(0),
                    
                    RemainLife = pet.RemainLife,
                    Attribute = pet.Attribute
                },
                _ => throw new ArgumentOutOfRangeException(nameof(item))
            }
        };
}
