using Edelstein.Common.Database;
using Edelstein.Common.Database.Entities.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edelstein.Common.Services.Auth.Tests;

[TestClass]
public partial class AuthServiceTests
{
    private readonly DbIdentityRepository repository;
    private readonly AuthService service;

    public AuthServiceTests()
    {
        repository = new DbIdentityRepository(
            new PooledDbContextFactory<GameDbContext>(new DbContextOptionsBuilder<GameDbContext>()
                .UseInMemoryDatabase("test")
                .Options),
            new Mapper()
        );
        service = new AuthService(repository);
    }
}
