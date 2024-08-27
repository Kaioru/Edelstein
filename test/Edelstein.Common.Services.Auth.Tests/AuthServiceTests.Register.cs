using Edelstein.Protocol.Services.Auth.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Edelstein.Common.Services.Auth.Tests;

public partial class AuthServiceTests
{
    [TestMethod]
    public async Task AuthServiceTests_Register_Failed_UsernameExists()
    {
        await repository.Insert(new Identity
        {
            Username = "testing",
            Password = string.Empty
        });
        
        var response = await service.Register(new AuthServiceRequest
        {
            Username = "testing",
            Password = "testing"
        });

        Assert.AreEqual(AuthServiceResult.FailedUsernameExists, response.Result);
    }
    
    [TestMethod]
    [DataRow("username1", "password")]
    [DataRow("uSeRnAmE2", "password")]
    [DataRow("username3", "PASSw0rd")]
    public async Task AuthServiceTests_Register_SuccessAndLoginAfter(string username, string password)
    {
        var registerResponse = await service.Register(new AuthServiceRequest
        {
            Username = username,
            Password = password
        });
        var loginResponse = await service.Login(new AuthServiceRequest
        {
            Username = username,
            Password = password
        });
        
        Assert.AreEqual(AuthServiceResult.Success, registerResponse.Result);
        Assert.AreEqual(AuthServiceResult.Success, loginResponse.Result);
    }
}
