namespace Edelstein.Protocol.Util.Tickers;

public interface ITickable
{
    Task OnTick(DateTime now);
}
