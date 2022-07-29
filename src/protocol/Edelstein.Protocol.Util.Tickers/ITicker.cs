namespace Edelstein.Protocol.Util.Tickers;

public interface ITicker
{
    Task Start();
    Task Stop();
}
