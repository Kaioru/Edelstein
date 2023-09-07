using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;

public interface IMobTemplate : ITemplate
{
    MoveAbilityType MoveAbility { get; }

    short Level { get; }
    
    bool IsBoss { get; }

    int MaxHP { get; }
    int MaxMP { get; }

    int PDD { get; }
    int PAD { get; }
    int PDR { get; }
    int MAD { get; }
    int MDD { get; }
    int MDR { get; }
    int ACC { get; }
    int EVA { get; }

    int EXP { get; }
    
    IDictionary<Element, ElementAttribute> ElementAttributes { get; }
}
