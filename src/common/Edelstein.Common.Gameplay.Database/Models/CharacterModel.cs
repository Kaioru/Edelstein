namespace Edelstein.Common.Gameplay.Database.Models;

public record CharacterModel : IDataModel
{
    public AccountWorldModel AccountWorld { get; }
    public string Name { get; set; }

    public int ID { get; set; }

    public int Version { get; set; }
    public byte[] Bytes { get; set; }
}
