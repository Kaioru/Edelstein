namespace Edelstein.Protocol.Gameplay.Game.Quests;

public enum QuestResultType
{
    StartQuestTimer = 0x6,
    EndQuestTimer = 0x7,

    StartTimeKeepQuestTimer = 0x8,
    EndTimeKeepQuestTimer = 0x9,

    Success = 0xA,

    FailedUnknown = 0xB,
    FailedInventory = 0xC,
    FailedMeso = 0xD,
    FailedPet = 0xE,
    FailedEquipped = 0xF,
    FailedOnlyItem = 0x10,
    FailedTimeOver = 0x11,
    
    ResetQuestTimer = 0x12
}
