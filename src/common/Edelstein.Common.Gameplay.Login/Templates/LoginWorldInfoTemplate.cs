using Duey.Abstractions;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Login.Templates;

public record LoginWorldInfoTemplate : ITemplate
{
    public int ID { get; }

    public string Name { get; }
    public byte State { get; }
    public bool BlockCharCreation { get; }

    public LoginWorldInfoTemplate(int id, IDataNode node)
    {
        ID = id;

        var cache = node.Cache();

        Name = cache.ResolveString("name") ?? "NO-NAME";
        State = cache.ResolveByte("state") ?? 0;
        BlockCharCreation = cache.ResolveBool("blockCharCreation") ?? false;
    }
}
