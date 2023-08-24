using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStageUserInitializer : IAdapterInitializer
{
    private readonly LoginContext _context;

    public LoginStageUserInitializer(LoginContext context) => _context = context;

    public IAdapter Initialize(ISocket socket) => new LoginStageUser(socket, _context);
}
