namespace Edelstein.Common.Gameplay.Database.Models;

public record AccountWorldModel : IDataModel
{
    public AccountModel Account { get; set; }

    public int AccountID { get; set; }
    public int WorldID { get; set; }

    public ICollection<CharacterModel> Characters { get; set; }

    public int ID { get; set; }

    public int Version { get; set; }
    public byte[] Bytes { get; set; }
}
