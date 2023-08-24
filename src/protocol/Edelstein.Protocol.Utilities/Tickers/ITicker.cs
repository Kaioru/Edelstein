namespace Edelstein.Protocol.Utilities.Tickers;

public interface ITicker
{
    int RefreshRate { get; }

    Task Start();
    Task Stop();
}
