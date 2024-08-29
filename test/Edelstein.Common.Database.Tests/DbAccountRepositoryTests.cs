using AutoMapper;
using Edelstein.Common.Database.Entities;
using Edelstein.Protocol.Gameplay.Entities;
using EntityFramework.Exceptions.Common;
using EntityFramework.Exceptions.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edelstein.Common.Database.Tests;

[TestClass]
public class DbAccountRepositoryTests
{
    private readonly SqliteConnection connection;
    private readonly DbAccountRepository repository;
    
    public DbAccountRepositoryTests()
    {
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var factory = new PooledDbContextFactory<GameDbContext>(new DbContextOptionsBuilder<GameDbContext>()
            .UseSqlite(connection)
            .UseExceptionProcessor()
            .Options);
        
        repository = new DbAccountRepository(
            factory,
            new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<GameDbMappingProfile>()))
        );
        
        factory.CreateDbContext().Database.EnsureCreated();
    }
    
    [TestCleanup]
    public void Cleanup() 
        => connection.Close();

    [TestMethod]
    public async Task DbAccountRepositoryTests_InsertDuplicateUsername()
    {
        await Assert.ThrowsExceptionAsync<UniqueConstraintException>(async () =>
        {
            await repository.Insert(new Account
            {
                Username = "user"
            });
            await repository.Insert(new Account
            {
                Username = "user"
            });
        });
    }

    [TestMethod]
    [DataRow("username1")]
    [DataRow("uSeRnAmE2")]
    [DataRow("username3")]
    public async Task DbAccountRepositoryTests_RetrieveByUsername(string username)
    {
        await repository.Insert(new Account
        {
            Username = username
        });

        Assert.IsNotNull(await repository.RetrieveByUsername(username));
    }
}
