using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;

public record  MobBurnedInfo(
    int CharacterID, 
    int SkillID,
    int Damage, 
    TimeSpan Interval,
    DateTime DateStart,
    DateTime DateExpire
) : IMobBurnedInfo;
