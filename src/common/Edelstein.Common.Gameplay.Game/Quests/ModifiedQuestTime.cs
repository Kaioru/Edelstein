using Edelstein.Protocol.Gameplay.Game.Quests;

namespace Edelstein.Common.Gameplay.Game.Quests;

public record ModifiedQuestTime(
    int ID, 
    DateTime DateStart, 
    DateTime DateEnd
) : IModifiedQuestTime;
