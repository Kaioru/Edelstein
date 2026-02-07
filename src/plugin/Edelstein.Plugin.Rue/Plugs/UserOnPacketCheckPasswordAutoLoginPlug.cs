using Edelstein.Plugin.Rue.Configs;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Plugin.Rue.Plugs;

public class UserOnPacketCheckPasswordAutoLoginPlug(ILogger? logger, IOptions<RueConfigLogin> options, LoginContext context) : IPipelinePlug<UserOnPacketCheckPassword>
{
    private readonly ILogger? _logger = logger;
    private readonly RueConfigLogin _config = options.Value;
    private readonly LoginContext _context = context;

    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckPassword message)
    {
        if (!_config.IsAutoRegister)
            return;

        var loginResult = await _context.Services.Auth.Login(new AuthRequest(message.Username, message.Password));

        if (loginResult.Result == AuthResult.FailedInvalidUsername)
        {
            await _context.Services.Auth.Register(new AuthRequest(message.Username, message.Password));
            _logger?.LogInformation("[Rue-AutoLogin] Auto-registered new account: {Username}", message.Username);
        }
    }
}
