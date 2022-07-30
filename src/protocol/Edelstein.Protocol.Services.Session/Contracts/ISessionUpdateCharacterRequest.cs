using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface ISessionUpdateCharacterRequest : IIdentifiable<int>
{
    public int? CharacterID { get; }
}
