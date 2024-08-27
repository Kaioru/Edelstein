using Edelstein.Common.Database;
using Edelstein.Common.Database.Entities.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edelstein.Common.Services.Auth.Tests;

[TestClass]
public class AuthServiceTests
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

    [TestMethod]
    public async Task AuthServiceTests_Login_Failed_InvalidUsername()
    {
        var response = await service.Login(new AuthServiceRequest
        {
            Username = "testing",
            Password = "testing"
        });

        Assert.AreEqual(AuthServiceResult.FailedInvalidUsername, response.Result);
    }
    
    [TestMethod]
    public async Task AuthServiceTests_Login_Failed_InvalidPassword()
    {
        await repository.Insert(new Identity
        {
            Username = "testing",
            Password = BCrypt.Net.BCrypt.HashPassword("testing")
        });
        
        var response = await service.Login(new AuthServiceRequest
        {
            Username = "testing",
            Password = "wrong"
        });

        Assert.AreEqual(AuthServiceResult.FailedInvalidPassword, response.Result);
    }
    
    [TestMethod]
    [DataRow("username1", "password")]
    [DataRow("uSeRnAmE2", "password")]
    [DataRow("username3", "PASSw0rd")]
    public async Task AuthServiceTests_Login_Success(string username, string password)
    {
        await repository.Insert(new Identity
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password)
        });
        
        var response = await service.Login(new AuthServiceRequest
        {
            Username = username,
            Password = password
        });
        
        Assert.AreEqual(AuthServiceResult.Success, response.Result);
    }
}
