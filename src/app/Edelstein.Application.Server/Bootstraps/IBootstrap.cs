namespace Edelstein.Application.Server.Bootstraps;

public interface IBootstrap
{
    int Priority { get; }

    Task Start();
    Task Stop();
}
