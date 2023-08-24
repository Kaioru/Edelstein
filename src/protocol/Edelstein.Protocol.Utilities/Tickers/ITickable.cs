namespace Edelstein.Protocol.Utilities.Tickers;

public interface ITickable
{
    Task OnTick(DateTime now);
}
