using System.Threading.Tasks;
using AutoMapper;
using Edelstein.Protocol.Gameplay.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Entities;

public class DbAccountWorldDataRepository(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper
) : DbRepository<int, DbAccountWorldData, AccountWorldData>(factory, mapper, db => db.AccountWorldData), 
    IAccountWorldDataRepository
{
    public async Task<AccountWorldData?> RetrieveByAccountAndWorld(int accountID, int worldID)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await db.AccountWorldData
            .FirstOrDefaultAsync(a => a.AccountID == accountID && a.WorldID == worldID);
        return entity != null ? mapper.Map<AccountWorldData>(entity) : null;
    }
}
