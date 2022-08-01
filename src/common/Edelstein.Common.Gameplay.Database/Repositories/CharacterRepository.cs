using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Database.Models;
using Edelstein.Common.Util.Serializers;
using Edelstein.Protocol.Gameplay.Characters;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Gameplay.Database.Repositories;

public class CharacterRepository : ICharacterRepository
{
    private readonly IDbContextFactory<GameplayDbContext> _dbFactory;
    private readonly ISerializer _serializer;

    public CharacterRepository(IDbContextFactory<GameplayDbContext> dbFactory, ISerializer serializer)
    {
        _dbFactory = dbFactory;
        _serializer = serializer;
    }

    public async Task<ICharacter?> Retrieve(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = await db.Characters.FindAsync(key);
        return model?.Adapt(_serializer.Deserialize<Character>(model.Bytes));
    }

    public async Task<ICharacter> Insert(ICharacter entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<CharacterModel>();
        model.Bytes = _serializer.Serialize(entry);
        db.Characters.Add(model);
        await db.SaveChangesAsync();
        return model.Adapt(_serializer.Deserialize<Character>(model.Bytes));
    }

    public async Task<ICharacter> Update(ICharacter entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = entry.Adapt<CharacterModel>();
        model.Bytes = _serializer.Serialize(entry);
        db.Characters.Update(model);
        await db.SaveChangesAsync();
        return model.Adapt(_serializer.Deserialize<Character>(model.Bytes));
    }

    public async Task Delete(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Characters.Remove(new CharacterModel { ID = key });
    }

    public async Task Delete(ICharacter entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Characters.Remove(new CharacterModel { ID = entry.ID });
    }

    public async Task<bool> CheckExistsByName(string name)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Characters.AnyAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public async Task<ICharacter?> RetrieveByName(string name)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = await db.Characters.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        return model?.Adapt(_serializer.Deserialize<Character>(model.Bytes));
    }

    public async Task<ICharacter?> RetrieveByAccountWorldAndCharacter(int accountWorld, int character)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var model = await db.Characters.FirstOrDefaultAsync(c => c.AccountWorldID == accountWorld && c.ID == character);
        return model?.Adapt(_serializer.Deserialize<Character>(model.Bytes));
    }

    public async Task<IEnumerable<ICharacter>> RetrieveAllByAccountWorld(int accountWorld)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var results = await db.Characters.Where(c => c.AccountWorldID == accountWorld).ToListAsync();
        return results
            .Select(m => m.Adapt(_serializer.Deserialize<Character>(m.Bytes)))
            .ToImmutableList();
    }
}
