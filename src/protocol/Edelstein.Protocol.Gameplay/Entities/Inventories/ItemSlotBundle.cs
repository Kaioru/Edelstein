namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record ItemSlotBundle : ItemSlotBase
{
    public short Number { get; set; }
    public short Attribute { get; set; }

    public string? Title { get; set; }
}
