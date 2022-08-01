namespace Edelstein.Common.Gameplay.Database.Models;

public record CharacterModel : IDataModel
{
    public AccountWorldModel AccountWorld { get; set; }

    public int AccountWorldID { get; set; }

    public string Name { get; set; }

    public int ID { get; set; }

    public byte[] Bytes { get; set; }
}
