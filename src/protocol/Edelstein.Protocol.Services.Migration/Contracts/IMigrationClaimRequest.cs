using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Services.Migration.Contracts;

public interface IMigrationClaimRequest : IIdentifiable<int>
{
    string Key { get; }
}
