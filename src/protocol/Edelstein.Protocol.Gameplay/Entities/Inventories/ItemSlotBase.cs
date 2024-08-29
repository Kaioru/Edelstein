using System;
using System.Text.Json.Serialization;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

[JsonDerivedType(typeof(ItemSlotBase), typeDiscriminator: "base")]
[JsonDerivedType(typeof(ItemSlotEquip), typeDiscriminator: "equip")]
[JsonDerivedType(typeof(ItemSlotBundle), typeDiscriminator: "bundle")]
[JsonDerivedType(typeof(ItemSlotPet), typeDiscriminator: "pet")]
public record ItemSlotBase
{
    public int TemplateID { get; set; }
    
    public DateTime? DateExpire { get; set; }
}
