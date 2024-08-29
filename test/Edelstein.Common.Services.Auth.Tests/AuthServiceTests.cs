using AutoMapper;
using Edelstein.Common.Database;
using Edelstein.Common.Database.Entities.Services.Auth;
using EntityFramework.Exceptions.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edelstein.Common.Services.Auth.Tests;

[TestClass]
public partial class AuthServiceTests
{
    private readonly SqliteConnection connection;
    private readonly DbIdentityRepository repository;
    private readonly AuthService service;

    public AuthServiceTests()
    {
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var factory = new PooledDbContextFactory<GameDbContext>(new DbContextOptionsBuilder<GameDbContext>()
            .UseSqlite(connection)
            .UseExceptionProcessor()
            .Options);
        
        repository = new DbIdentityRepository(
            factory,
            new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<GameDbMappingProfile>()))
        );
        service = new AuthService(repository);

        factory.CreateDbContext().Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup() 
        => connection.Close();
}
