using Edelstein.Common.Gameplay.Accounts;

namespace Edelstein.Common.Database.Entities;

public record AccountWorldEntity : AccountWorld
{
    public AccountEntity Account { get; set; }
    public ICollection<CharacterEntity> Characters { get; set; }
}
