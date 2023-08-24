using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay;

public interface IStage<TStageUser> : IIdentifiable<string>
    where TStageUser : IStageUser<TStageUser>
{
    IReadOnlyRepository<int, TStageUser> Users { get; }

    Task Enter(TStageUser user);
    Task Leave(TStageUser user);
}
