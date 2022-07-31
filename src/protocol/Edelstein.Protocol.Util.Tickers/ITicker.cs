namespace Edelstein.Protocol.Util.Tickers;

public interface ITicker
{
    int RefreshRate { get; }

    Task Start();
    Task Stop();
}
