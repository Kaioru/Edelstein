using Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats.TwoState;

public record TwoStateGuidedBulletRecord : TwoStateTemporaryStatRecord, ITwoStateGuidedBulletRecord
{
    public int MobID { get; set; }
}
