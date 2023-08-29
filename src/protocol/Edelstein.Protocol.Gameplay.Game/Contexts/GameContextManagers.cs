using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Objects.User.Stats;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextManagers(
    IDataManager Data,
    ITickerManager Ticker,
    
    IFieldManager Field,
    IContiMoveManager ContiMove,
    INamedConversationManager Conversation,
    
    IFieldUserStatsCalculator UserStats
);
