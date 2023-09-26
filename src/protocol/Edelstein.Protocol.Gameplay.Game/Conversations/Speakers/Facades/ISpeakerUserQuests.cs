namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;

public interface ISpeakerUserQuests
{
    void SetRecord(short questID, string value);
    void SetRecord(short questID, string key, string value);
    void SetRecordEx(short questID, string value);
    void SetRecordEx(short questID, string key, string value);

    string GetRecord(short questID);
    string GetRecord(short questID, string key);
    string GetRecordEx(short questID);
    string GetRecordEx(short questID, string key);

    int GetStatus(short questID);

    void Accept(short questID);
    void Complete(short questID);
    void Resign(short questID);
}
