using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Common.Gameplay.Models.Characters.Quests;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers.Facades;

public class SpeakerUserQuests : ISpeakerUserQuests
{
    private readonly IFieldUser _user;

    public SpeakerUserQuests(IFieldUser user) => _user = user;

    private IDictionary<string, string> GetDictionaryFromRecord(string record) 
        => record
            .Split(";")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Split("="))
            .Select(s => Tuple.Create(s.Length > 0 ? s[0] : string.Empty, s.Length > 1 ? s[1] : string.Empty))
            .DistinctBy(t => t.Item1)
            .ToDictionary(
                s => s.Item1,
                s => s.Item2
            );

    private string GetRecordFromDictionary(IDictionary<string, string> dictionary)
        => string.Join(';', dictionary
            .Select(kv => kv.Key + "=" + kv.Value)
            .ToImmutableHashSet());

    public void SetRecord(short questID, string value)
    {
        if (_user.Character.QuestRecords.Records.TryGetValue(questID, out var record))
            record.Value = value;
        else 
            _user.Character.QuestRecords.Records[questID] = new QuestRecord {Value = value};
        _user.Message(new QuestRecordUpdateMessage(questID, value)).Wait();
    }
    
    public void SetRecord(short questID, string key, string value)
    {
        var record = GetRecord(questID);
        var dictionary = GetDictionaryFromRecord(record);

        dictionary[key] = value;
        SetRecord(questID, GetRecordFromDictionary(dictionary));
    }

    public void SetRecordEx(short questID, string value)
    {
        if (_user.Character.QuestRecordsEx.Records.TryGetValue(questID, out var record))
            record.Value = value;
        else 
            _user.Character.QuestRecordsEx.Records[questID] = new QuestRecordEx {Value = value};
        _user.Message(new QuestRecordExMessage(questID, value)).Wait();
    }
    
    public void SetRecordEx(short questID, string key, string value)
    {
        var record = GetRecordEx(questID);
        var dictionary = GetDictionaryFromRecord(record);

        dictionary[key] = value;
        SetRecord(questID, GetRecordFromDictionary(dictionary));
    }

    public string GetRecord(short questID) 
        => _user.Character.QuestRecords[questID]?.Value ?? string.Empty;

    public string GetRecord(short questID, string key)
        => GetDictionaryFromRecord(GetRecord(questID)).TryGetValue(key, out var value) ? value : string.Empty;

    public string GetRecordEx(short questID)
        => _user.Character.QuestRecordsEx[questID]?.Value ?? string.Empty;
    
    public string GetRecordEx(short questID, string key)
        => GetDictionaryFromRecord(GetRecordEx(questID)).TryGetValue(key, out var value) ? value : string.Empty;
    
    public int GetStatus(short questID) 
        => _user.Character.QuestCompletes.Records.ContainsKey(questID)
            ? 2
            : _user.Character.QuestRecords.Records.ContainsKey(questID)
                ? 1
                : 0;

    public void Accept(short questID) => throw new NotImplementedException();
    public void Complete(short questID) => throw new NotImplementedException();
    public void Resign(short questID) => throw new NotImplementedException();
}
