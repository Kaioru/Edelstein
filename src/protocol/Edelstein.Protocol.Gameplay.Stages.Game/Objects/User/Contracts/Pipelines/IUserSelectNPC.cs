using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;

public interface IUserSelectNPC : IFieldUserContract
{
    IFieldNPC NPC { get; }
}
