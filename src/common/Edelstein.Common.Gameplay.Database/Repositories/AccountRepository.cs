using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Accounts;

namespace Edelstein.Common.Gameplay.Database.Repositories;

public class AccountRepository : IAccountRepository
{
    private static readonly IAccount account = new Account { ID = 1, Username = "Testing" };

    public Task<IAccount?> Retrieve(int key) => Task.FromResult<IAccount?>(account);
    public Task<IAccount> Insert(IAccount entry) => Task.FromResult(account);
    public Task<IAccount> Update(IAccount entry) => Task.FromResult(account);
    public Task Delete(int key) => Task.CompletedTask;
    public Task Delete(IAccount entry) => Task.CompletedTask;

    public Task<IAccount?> RetrieveByUsername(string username) => Task.FromResult<IAccount?>(account);
}
