using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Database.Models;
using Edelstein.Protocol.Gameplay.Accounts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Gameplay.Database.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDbContextFactory<GameplayDbContext> _dbFactory;

    public AccountRepository(IDbContextFactory<GameplayDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<IAccount?> Retrieve(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return (await db.Accounts.FindAsync(key))?.Adapt<Account>();
    }

    public async Task<IAccount> Insert(IAccount entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<AccountModel>();
        db.Accounts.Add(model);
        await db.SaveChangesAsync();
        return model.Adapt<Account>();
    }

    public async Task<IAccount> Update(IAccount entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<AccountModel>();
        db.Accounts.Update(model);
        await db.SaveChangesAsync();
        return model.Adapt<Account>();
    }

    public async Task Delete(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Accounts.Remove(new AccountModel { ID = key });
        await db.SaveChangesAsync();
    }

    public async Task Delete(IAccount entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Accounts.Remove(new AccountModel { ID = entry.ID });
        await db.SaveChangesAsync();
    }

    public async Task<IAccount?> RetrieveByUsername(string username)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return (await db.Accounts.FirstOrDefaultAsync(a => a.Username.ToLower().Equals(username.ToLower())))
            ?.Adapt<Account>();
    }
}
