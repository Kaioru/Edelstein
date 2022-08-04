using Edelstein.Protocol.Data;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContextManagers
{
    IDataManager Data { get; }
    ITickerManager Ticker { get; }
}
