using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Services.Auth;

public interface IIdentityRepository : IQueriedRepository<int, Identity>
{
    Task<Identity?> RetrieveByUsername(string username);
}
