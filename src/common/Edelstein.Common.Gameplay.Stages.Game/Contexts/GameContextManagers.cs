using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Commands;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Common.Gameplay.Stages.Game.Contexts;

public record GameContextManagers(
    IDataManager Data,
    ITickerManager Ticker,
    ICommandManager<IFieldUser> Command
) : IGameContextManagers;
