using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;

public interface INPCStringTemplate : ITemplate
{
    string Name { get; }
    string Func { get; }
}
