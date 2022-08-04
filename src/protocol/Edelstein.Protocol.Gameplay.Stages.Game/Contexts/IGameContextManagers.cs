﻿using Edelstein.Protocol.Data;
using Edelstein.Protocol.Util.Commands;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextManagers
{
    IDataManager Data { get; }
    ITickerManager Ticker { get; }
    ICommandManager<IFieldUser> Command { get; }
}
