using Edelstein.Common.Gameplay.Models.Characters;

namespace Edelstein.Common.Database.Entities;

public record CharacterEntity : Character
{
    public AccountWorldEntity AccountWorld { get; set; }
}
