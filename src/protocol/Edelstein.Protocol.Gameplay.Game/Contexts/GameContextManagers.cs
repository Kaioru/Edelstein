using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextManagers(
    IDataManager Data,
    ITickerManager Ticker,
    
    IInventoryManager Inventory,
    IFieldManager Field,
    IContiMoveManager ContiMove,
    INamedConversationManager Conversation,
    ISkillManager Skill,
    IQuestManager Quest,
    IModifiedQuestTimeManager QuestTime
);
