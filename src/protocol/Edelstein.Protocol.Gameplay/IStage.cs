using Edelstein.Protocol.Util.Storages;

namespace Edelstein.Protocol.Gameplay;

public interface IStage : IIdentifiable<string>
{
    IReadOnlyStorage<int, IStageUser> Users { get; }

    Task Enter(IStageUser user);
    Task Leave(IStageUser user);
}
