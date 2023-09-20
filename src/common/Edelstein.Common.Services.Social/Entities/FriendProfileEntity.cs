namespace Edelstein.Common.Services.Social.Entities;

public class FriendProfileEntity
{
    public int CharacterID { get; set; }
    public byte FriendMax { get; set; }
    public bool IsMaster { get; set; }

    public ICollection<FriendEntity> Friends { get; set; } = new List<FriendEntity>();
}
