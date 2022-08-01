using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Database.Models;
using Edelstein.Common.Gameplay.Database.Serializers;
using Edelstein.Protocol.Gameplay.Accounts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Gameplay.Database.Repositories;

public class AccountWorldRepository : IAccountWorldRepository
{
    private readonly IDbContextFactory<GameplayDbContext> _dbFactory;
    private readonly ISerializer _serializer;

    public AccountWorldRepository(IDbContextFactory<GameplayDbContext> dbFactory, ISerializer serializer)
    {
        _dbFactory = dbFactory;
        _serializer = serializer;
    }

    public async Task<IAccountWorld?> Retrieve(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = await db.AccountWorlds.FindAsync(key);
        return model?.Adapt(_serializer.Deserialize<AccountWorld>(model.Bytes));
    }

    public async Task<IAccountWorld> Insert(IAccountWorld entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<AccountWorldModel>();
        model.Bytes = _serializer.Serialize(entry);
        db.AccountWorlds.Add(model);
        await db.SaveChangesAsync();
        return model.Adapt(_serializer.Deserialize<AccountWorld>(model.Bytes));
    }

    public async Task<IAccountWorld> Update(IAccountWorld entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<AccountWorldModel>();
        db.AccountWorlds.Update(model);
        await db.SaveChangesAsync();
        return model.Adapt(_serializer.Deserialize<AccountWorld>(model.Bytes));
    }

    public async Task Delete(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Remove(new AccountWorldModel { ID = key });
    }

    public async Task Delete(IAccountWorld entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Remove(entry.Adapt<AccountWorldModel>());
    }

    public async Task<IAccountWorld?> RetrieveByAccountAndWorld(int accountID, int worldID)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = await db.AccountWorlds.FirstOrDefaultAsync(a => a.AccountID == accountID && a.WorldID == worldID);
        return model?.Adapt(_serializer.Deserialize<AccountWorld>(model.Bytes));
    }
}
