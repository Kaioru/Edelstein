using Edelstein.Plugin.Rue.Configs;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.Plugs;

public class UserOnPacketCheckPasswordAutoLoginPlug : IPipelinePlug<UserOnPacketCheckPassword>
{
    private readonly ILogger? _logger;
    private readonly RueConfigLogin? _config;
    private readonly LoginContext _context;

    public UserOnPacketCheckPasswordAutoLoginPlug(ILogger? logger, RueConfigLogin? config, LoginContext context)
    {
        _logger = logger;
        _config = config;
        _context = context;
    }
    
    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckPassword message)
    {
        if ((_config?.IsAutoRegister ?? false) && 
            (await _context.Services.Auth.Login(new AuthRequest(message.Username, message.Password))).Result == AuthResult.FailedInvalidUsername)
        {
            await _context.Services.Auth.Register(new AuthRequest(message.Username, message.Password));
            _logger?.LogInformation("Created new user {Username} with password {Password}", message.Username, message.Password);
        }
    }
}
