namespace Edelstein.Common.Gameplay.Game.Combat.Contexts;

public record SkillContextTwoStatePartyBooster(
    int Value,
    int Reason,
    DateTime DateStart,
    TimeSpan Term
);
