using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob;

public interface IFieldMobStats
{
    int Level { get; }
    
    int PAD { get; }
    int PDD { get; }
    int PDR { get; }
    int MAD { get; }
    int MDD { get; }
    int MDR { get; }

    int ACC { get; }
    int EVA { get; }
    
    IDictionary<Element, ElementAttribute> ElementAttributes { get; }
}
