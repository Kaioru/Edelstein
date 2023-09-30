using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Contracts;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.Plugs;

public class UserOnPacketCreateSecurityHandleAutoRegisterPlug : IPipelinePlug<UserOnPacketCreateSecurityHandle>
{
    private readonly ILogger? _logger;
    private readonly RueConfigLogin? _config;
    private readonly LoginContext _context;
    
    public UserOnPacketCreateSecurityHandleAutoRegisterPlug(ILogger? logger, RueConfigLogin? config, LoginContext context)
    {
        _logger = logger;
        _config = config;
        _context = context;
    }

    public async Task Handle(IPipelineContext ctx, UserOnPacketCreateSecurityHandle message)
    {
        if ((_config?.IsAutoLogin ?? false) && _config.LoginCredentials is { Username: not null, Password: not null })
        {
            _logger?.LogInformation(
                "Logging in user {Username} with password {Password}", 
                _config.LoginCredentials.Username, _config.LoginCredentials.Password
            );

            await _context.Pipelines.UserOnPacketCheckPassword.Process(new UserOnPacketCheckPasswordFlipped(
                message.User,
                _config.LoginCredentials.Username, 
                _config.LoginCredentials.Password
            ));
        }
    }
}
