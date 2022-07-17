using Edelstein.Common.Services.Auth.Contracts;
using Edelstein.Common.Services.Auth.Models;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IDbContextFactory<AuthDbContext> _dbFactory;

    public AuthService(IDbContextFactory<AuthDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<IAuthLoginResponse> Login(IAuthLoginRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var identity = await db.Identities
                .FirstOrDefaultAsync(i => i.Username.ToLower().Equals(request.Username.ToLower()));

            if (identity == null)
                return new AuthLoginResponse(AuthLoginResult.FailedInvalidUsername);
            if (!BCrypt.Net.BCrypt.Verify(request.Password, identity.Password))
                return new AuthLoginResponse(AuthLoginResult.FailedInvalidPassword);

            return new AuthLoginResponse(AuthLoginResult.Success);
        }
        catch (Exception)
        {
            return new AuthLoginResponse(AuthLoginResult.FailedUnknown);
        }
    }

    public async Task<IAuthRegisterResponse> Register(IAuthRegisterRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            if (await db.Identities.AnyAsync(i => i.Username.ToLower().Equals(request.Username.ToLower())))
                return new AuthRegisterResponse(AuthRegisterResult.FailedUsernameExists);

            db.Identities.Add(new IdentityModel
            {
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            });
            await db.SaveChangesAsync();

            return new AuthRegisterResponse(AuthRegisterResult.Success);
        }
        catch (Exception)
        {
            return new AuthRegisterResponse(AuthRegisterResult.FailedUnknown);
        }
    }
}
