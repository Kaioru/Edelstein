using System.Runtime.Serialization;

namespace Edelstein.Protocol.Services.Server.Contracts;

[DataContract]
public record ServerEntryLogin : ServerEntry, IServerEntryLogin;
