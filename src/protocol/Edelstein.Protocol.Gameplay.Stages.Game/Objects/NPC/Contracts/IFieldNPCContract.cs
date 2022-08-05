using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Contracts;

public interface IFieldNPCContract : IFieldUserContract
{
    IFieldNPC NPC { get; }
}
