using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Login.Templates;

namespace Edelstein.Common.Gameplay.Login.Templates;

public record WorldTemplate : IWorldTemplate
{

    public WorldTemplate(int id, IDataNode node)
    {
        ID = id;

        Name = node.ResolveString("name") ?? "NO-NAME";
        State = node.ResolveByte("state") ?? 0;
        BlockCharCreation = node.ResolveBool("blockCharCreation") ?? false;
    }
    public int ID { get; }

    public string Name { get; }
    public byte State { get; }
    public bool BlockCharCreation { get; }
}
