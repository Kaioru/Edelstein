using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Edelstein.Protocol.Services.Migration.Contracts;

[DataContract]
public record MigrationInfoSnapshot<T>
{
    [DataMember(Order = 1)] public string? Data { get; init; }

    [JsonIgnore]
    public T? Value => Data != null ? JsonSerializer.Deserialize<T>(Data) : default;
    
    public MigrationInfoSnapshot() {}
    public MigrationInfoSnapshot(T entry) : this() 
        => Data = JsonSerializer.Serialize(entry);
}
