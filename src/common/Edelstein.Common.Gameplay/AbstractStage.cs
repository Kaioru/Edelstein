using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Gameplay;

public abstract class AbstractStage<TStageUser> : IStage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{

    private readonly IRepository<int, TStageUser> _users;

    protected AbstractStage()
        => _users = new Repository<int, TStageUser>();
    public abstract string ID { get; }

    public IReadOnlyRepository<int, TStageUser> Users => _users;

    public async Task Enter(TStageUser user)
    {
        user.Stage = this;
        await _users.Insert(user);
    }

    public async Task Leave(TStageUser user)
    {
        user.Stage = null;
        await _users.Delete(user);
    }
}
