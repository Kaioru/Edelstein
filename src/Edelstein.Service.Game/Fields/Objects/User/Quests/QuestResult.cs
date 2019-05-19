namespace Edelstein.Service.Game.Fields.Objects.User.Quests
{
    public enum QuestResult
    {
        StartQuestTimer = 0x6,
        EndQuestTimer = 0x7,
        StartTimeKeepQuestTimer = 0x8,
        EndTimeKeepQuestTimer = 0x9,
        ActSuccess = 0xA,
        ActFailedUnknown = 0xB,
        ActFailedInventory = 0xC,
        ActFailedMeso = 0xD,
        ActFailedPet = 0xE,
        ActFailedEquipped = 0xF,
        ActFailedOnlyItem = 0x10,
        ActFailedTimeOver = 0x11,
        ActResetQuestTimer = 0x12
    }
}