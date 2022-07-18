namespace Edelstein.Protocol.Services.Session;

public interface ISessionService
{
    Task Start();
    Task Update();
    Task End();
}
