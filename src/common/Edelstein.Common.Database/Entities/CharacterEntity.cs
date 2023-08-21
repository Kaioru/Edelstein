using Edelstein.Common.Gameplay.Characters;

namespace Edelstein.Common.Database.Entities;

public record CharacterEntity : Character
{
    public AccountWorldEntity AccountWorld { get; set; }
}
