using System.Collections.Immutable;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCTemplate : INPCTemplate
{

    public NPCTemplate(int id, IDataNode node, IDataNode info)
    {
        ID = id;

        Move = node.ResolvePath("move") != null;

        IsStoreBank = info.ResolveBool("storeBank") ?? false;
        IsParcel = info.ResolveBool("parcel") ?? false;

        TrunkPut = info.ResolveInt("trunkPut") ?? 0;
        TrunkGet = info.ResolveInt("trunkGet") ?? 0;

        Scripts = info.ResolvePath("script")?.Children
                      .Where(p => p.Name.All(char.IsDigit)) // 1057006 causes errors
                      .Select(p => new NPCTemplateScript(Convert.ToInt32(p.Name), p))
                      .ToImmutableList()
                  ?? ImmutableList<NPCTemplateScript>.Empty;
    }
    public int ID { get; }

    public bool Move { get; }

    public bool IsTrunk => TrunkPut > 0 || TrunkGet > 0;
    public bool IsStoreBank { get; }
    public bool IsParcel { get; }

    public int TrunkPut { get; }
    public int TrunkGet { get; }

    public IReadOnlyCollection<INPCTemplateScript> Scripts { get; }
}
