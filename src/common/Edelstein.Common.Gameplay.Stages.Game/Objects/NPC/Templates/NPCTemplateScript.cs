using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Templates;

public record NPCTemplateScript : INPCTemplateScript
{
    public NPCTemplateScript(int id, IDataProperty property) =>
        Script = property.ResolveOrDefault<string>("script") ?? "NO-SCRIPT";
    // TODO: start end dates

    public string Script { get; }
}
