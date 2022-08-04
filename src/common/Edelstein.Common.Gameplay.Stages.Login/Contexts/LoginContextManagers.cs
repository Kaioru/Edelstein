using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContextManagers(
    IDataManager Data,
    ITickerManager Ticker
) : ILoginContextManagers;
