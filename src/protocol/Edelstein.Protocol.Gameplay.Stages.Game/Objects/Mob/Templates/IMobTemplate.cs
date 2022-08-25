using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;

public interface IMobTemplate : ITemplate
{
    MoveAbilityType MoveAbility { get; }

    short Level { get; }

    int MaxHP { get; }
    int MaxMP { get; }

    int PAD { get; }
    int PDR { get; }
    int MAD { get; }
    int MDR { get; }
    int ACC { get; }
    int EVA { get; }

    int EXP { get; }
}
