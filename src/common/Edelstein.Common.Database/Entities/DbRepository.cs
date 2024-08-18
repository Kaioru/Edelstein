using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Repositories;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Entities;

public class DbRepository<TEntityDb, TEntityDto>(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper,
    Func<GameDbContext, DbSet<TEntityDb>> selector
) : IQueriedRepository<int, TEntityDto> 
    where TEntityDb : class, IRepositoryEntry<int>, TEntityDto
    where TEntityDto : class, IRepositoryEntry<int>
{
    public async Task<TEntityDto?> Retrieve(int key)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await selector.Invoke(db).FindAsync(key);
        return entity != null ? mapper.Map<TEntityDto>(entity) : default;
    }

    public async Task<TEntityDto> Insert(TEntityDto entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = mapper.Map<TEntityDb>(entry);
        selector.Invoke(db).Add(entity);
        await db.SaveChangesAsync();
        return mapper.Map<TEntityDto>(entity);
    }

    public async Task<TEntityDto> Update(TEntityDto entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = mapper.Map<TEntityDb>(entry);
        selector.Invoke(db).Update(entity);
        await db.SaveChangesAsync();
        return mapper.Map<TEntityDto>(entity);
    }

    public async Task Delete(int key)
    {
        await using var db = await factory.CreateDbContextAsync();
        await selector.Invoke(db).Where(a => a.ID == key).ExecuteDeleteAsync();
    }
    
    public async Task Delete(TEntityDto entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        await selector.Invoke(db).Where(a => a.ID == entry.ID).ExecuteDeleteAsync();
    }
}
