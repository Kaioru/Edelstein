using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;

public interface INPCTemplate : ITemplate
{
    bool Move { get; }

    bool IsTrunk { get; }
    bool IsStoreBank { get; }
    bool IsParcel { get; }

    int TrunkPut { get; }
    int TrunkGet { get; }

    IReadOnlyCollection<INPCTemplateScript> Scripts { get; }
}
