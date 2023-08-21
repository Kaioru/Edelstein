using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDbContextFactory<GameplayDbContext> _dbFactory;

    public AccountRepository(IDbContextFactory<GameplayDbContext> dbFactory) 
        => _dbFactory = dbFactory;


    public async Task<IAccount?> Retrieve(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return (await db.Accounts.FindAsync(key))?.Adapt<Account>();
    }

    public async Task<IAccount> Insert(IAccount entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<AccountEntity>();
        db.Accounts.Add(model);
        await db.SaveChangesAsync();
        return model.Adapt<Account>();
    }

    public async Task<IAccount> Update(IAccount entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<AccountEntity>();
        db.Accounts.Update(model);
        await db.SaveChangesAsync();
        return model.Adapt<Account>();
    }

    public async Task Delete(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Accounts.Remove(new AccountEntity { ID = key });
        await db.SaveChangesAsync();
    }

    public async Task Delete(IAccount entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Accounts.Remove(new AccountEntity { ID = entry.ID });
        await db.SaveChangesAsync();
    }

    public async Task<IAccount?> RetrieveByUsername(string username)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return (await db.Accounts
            .FirstOrDefaultAsync(a => a.Username.Equals(username)))?
            .Adapt<Account>();
    }
}
