using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Login.Templates;

namespace Edelstein.Common.Gameplay.Stages.Login.Templates;

public class WorldTemplate : IWorldTemplate
{
    public WorldTemplate(int id, IDataProperty property)
    {
        ID = id;

        Name = property.ResolveOrDefault<string>("name") ?? "NO-NAME";
        State = property.Resolve<byte>("state") ?? 0;
        BlockCharCreation = property.Resolve<bool>("blockCharCreation") ?? false;
    }

    public int ID { get; }

    public string Name { get; }
    public byte State { get; }
    public bool BlockCharCreation { get; }
}
