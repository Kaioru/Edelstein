namespace Edelstein.Protocol.Gameplay.Models.Characters.Quests.Modify;

public interface IModifyQuestContext
{
    void SetRecord(int questID, string record);
    void SetRecordEx(int questID, string recordEx);
    
    void ResetRecord(int questID);
    void ResetRecordEx(int questID);
    
    void Complete(int questID);
    void Reset(int questID);
}
