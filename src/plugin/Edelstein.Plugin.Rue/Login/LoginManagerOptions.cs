namespace Edelstein.Plugin.Rue.Login;

public class LoginManagerOptions : ILoginManagerOptions
{
    public LoginManagerOptions(
        bool skipAuthorization,
        bool isAutoRegister,
        bool autoLogin,
        (string, string) autoLoginCredentials)
    {
        SkipAuthorization = skipAuthorization;
        IsAutoRegister = isAutoRegister;
        AutoLogin = autoLogin;
        AutoLoginCredentials = autoLoginCredentials;
    }

    public bool SkipAuthorization { get; set; }
    public bool IsAutoRegister { get; set; }
    public bool AutoLogin { get; set; }
    public (string, string) AutoLoginCredentials { get; set; }
}
