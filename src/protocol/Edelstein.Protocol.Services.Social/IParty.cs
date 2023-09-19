using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Services.Social;

public interface IParty : IIdentifiable<int>
{
    int BossCharacterID { get; }
}
