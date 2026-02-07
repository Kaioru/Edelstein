using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Edelstein.Plugin.Rue.Configs;

public static class RueConfigBinding
{
    public const string LoginSectionName = "Rue:Login";
    public const string GameSectionName = "Rue:Game";

    public static IOptions<RueConfigLogin> BindLoginOptions(IConfiguration config)
    {
        var settings = new RueConfigLogin();
        var section = config.GetSection(LoginSectionName);

        if (section.Exists())
            section.Bind(settings);
        else
            config.Bind(settings);

        return Options.Create(settings);
    }

    public static IOptions<RueConfigGame> BindGameOptions(IConfiguration config)
    {
        var settings = new RueConfigGame();
        var section = config.GetSection(GameSectionName);

        if (section.Exists())
            section.Bind(settings);
        else
            config.Bind(settings);

        return Options.Create(settings);
    }
}
