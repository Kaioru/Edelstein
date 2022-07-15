using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Stages.Login;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStage : ILoginStage
{
    private readonly IDictionary<int, ILoginStageUser> _users;

    public string ID => "Login-0";

    public LoginStage()
    {
        _users = new Dictionary<int, ILoginStageUser>();
    }

    public IEnumerable<ILoginStageUser> GetUsers() => _users.Values.ToImmutableList();

    public Task Enter(ILoginStageUser user)
    {
        _users.Add(user.ID, user);
        return Task.CompletedTask;
    }

    public Task Leave(ILoginStageUser user)
    {
        _users.Remove(user.ID);
        return Task.CompletedTask;
    }
}
