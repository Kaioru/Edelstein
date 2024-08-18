using Edelstein.Protocol.Services.Auth;

namespace Edelstein.Common.Services.Auth;

public partial class AuthService(
    IIdentityRepository repository
) : IAuthService;
