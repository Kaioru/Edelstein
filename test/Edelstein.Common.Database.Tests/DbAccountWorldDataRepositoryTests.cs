using Edelstein.Common.Database.Entities;
using Edelstein.Protocol.Gameplay.Entities;
using EntityFramework.Exceptions.Common;
using EntityFramework.Exceptions.Sqlite;
using MapsterMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edelstein.Common.Database.Tests;

[TestClass]
public class DbAccountWorldDataRepositoryTests
{
    private readonly SqliteConnection connection;
    private readonly DbAccountRepository accounts;
    private readonly DbAccountWorldDataRepository repository;
    
    public DbAccountWorldDataRepositoryTests()
    {
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var factory = new PooledDbContextFactory<GameDbContext>(new DbContextOptionsBuilder<GameDbContext>()
            .UseSqlite(connection)
            .UseExceptionProcessor()
            .Options);
        
        accounts = new DbAccountRepository(
            factory,
            new Mapper()
        );
        repository = new DbAccountWorldDataRepository(
            factory,
            new Mapper()
        );
        
        factory.CreateDbContext().Database.EnsureCreated();
    }
    
    [TestCleanup]
    public void Cleanup() 
        => connection.Close();
    
    [TestMethod]
    public async Task DbAccountWorldDataRepositoryTests_InsertWithoutAccount()
    {
        await Assert.ThrowsExceptionAsync<ReferenceConstraintException>(async () =>
        {
            await repository.Insert(new AccountWorldData
            {
                AccountID = 0,
                WorldID = 0
            });
        });
    }
    
    [TestMethod]
    public async Task DbAccountWorldDataRepositoryTests_InsertDuplicateAccountAndWorld()
    {
        var account = await accounts.Insert(new Account
        {
            Username = "user"
        });
        
        await Assert.ThrowsExceptionAsync<UniqueConstraintException>(async () =>
        {
            await repository.Insert(new AccountWorldData
            {
                AccountID = account.ID,
                WorldID = 0
            });
            await repository.Insert(new AccountWorldData
            {
                AccountID = account.ID,
                WorldID = 0
            });
        });
    }

    [TestMethod]
    [DataRow("username1", 0)]
    [DataRow("uSeRnAmE2", 1)]
    [DataRow("username3", 2)]
    public async Task DbAccountWorldDataRepositoryTests_RetrieveByAccountAndWorld(string username, int world)
    {
        var account = await accounts.Insert(new Account
        {
            Username = username
        });
        
        Assert.IsNull(await repository.RetrieveByAccountAndWorld(account.ID, world));
        
        await repository.Insert(new AccountWorldData
        {
            AccountID = account.ID,
            WorldID = world
        });
        
        Assert.IsNotNull(await repository.RetrieveByAccountAndWorld(account.ID, world));
    }
}
