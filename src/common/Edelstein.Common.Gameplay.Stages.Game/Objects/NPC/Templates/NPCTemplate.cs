using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Templates;

public record NPCTemplate : INPCTemplate
{
    public NPCTemplate(int id, IDataProperty property, IDataProperty info)
    {
        ID = id;

        Move = property.Resolve("move") != null;

        IsStoreBank = info.Resolve<bool>("storeBank") ?? false;
        IsParcel = info.Resolve<bool>("parcel") ?? false;

        TrunkPut = info.Resolve<int>("trunkPut") ?? 0;
        TrunkGet = info.Resolve<int>("trunkGet") ?? 0;

        Scripts = info.Resolve("script")?.Children
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
