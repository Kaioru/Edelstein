using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Repositories;

public class AccountWorldRepository : IAccountWorldRepository
{
    private readonly IDbContextFactory<GameplayDbContext> _dbFactory;
    
    public AccountWorldRepository(IDbContextFactory<GameplayDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<IAccountWorld?> Retrieve(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return (await db.AccountWorlds.FindAsync(key))?.Adapt<AccountWorld>();
    }

    public async Task<IAccountWorld> Insert(IAccountWorld entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<AccountWorldEntity>();
        db.AccountWorlds.Add(model);
        await db.SaveChangesAsync();
        return model.Adapt<AccountWorld>();
    }

    public async Task<IAccountWorld> Update(IAccountWorld entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<AccountWorldEntity>();
        db.AccountWorlds.Update(model);
        await db.SaveChangesAsync();
        return model.Adapt<AccountWorld>();
    }

    public async Task Delete(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.AccountWorlds.Remove(new AccountWorldEntity { ID = key });
        await db.SaveChangesAsync();
    }

    public async Task Delete(IAccountWorld entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.AccountWorlds.Remove(new AccountWorldEntity { ID = entry.ID });
        await db.SaveChangesAsync();
    }

    public async Task<IAccountWorld?> RetrieveByAccountAndWorld(int accountID, int worldID)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return (await db.AccountWorlds
                .FirstOrDefaultAsync(a => a.AccountID == accountID && a.WorldID == worldID))?
            .Adapt<AccountWorld>();
    }
}
