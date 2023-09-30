namespace Edelstein.Plugin.Rue.Login;

public interface ILoginManagerOptions
{
    bool SkipAuthorization { get; set; }
    bool AutoLogin { get; set; }
    (string, string) AutoLoginCredentials { get; set; }
}
