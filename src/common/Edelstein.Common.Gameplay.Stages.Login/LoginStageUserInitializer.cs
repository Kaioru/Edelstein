using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStageUserInitializer : IAdapterInitializer
{
    private readonly ILoginContext _context;

    public LoginStageUserInitializer(ILoginContext context) => _context = context;

    public IAdapter Initialize(ISocket socket) => new LoginStageUser(socket, _context);
}
