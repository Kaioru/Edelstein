using Edelstein.Common.Services.Auth.Entities;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IDbContextFactory<AuthDbContext> _dbFactory;

    public AuthService(IDbContextFactory<AuthDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var identity = await db.Identities
                .FirstOrDefaultAsync(i => i.Username.ToLower().Equals(request.Username.ToLower()));

            if (identity == null)
                return new AuthResponse(AuthResult.FailedInvalidUsername);
            if (!BCrypt.Net.BCrypt.Verify(request.Password, identity.Password))
                return new AuthResponse(AuthResult.FailedInvalidPassword);

            return new AuthResponse(AuthResult.Success);
        }
        catch (Exception)
        {
            return new AuthResponse(AuthResult.FailedUnknown);
        }
    }

    public async Task<AuthResponse> Register(AuthRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            if (await db.Identities.AnyAsync(i => i.Username.ToLower().Equals(request.Username.ToLower())))
                return new AuthResponse(AuthResult.FailedUsernameExists);

            db.Identities.Add(new IdentityEntity
            {
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            });
            await db.SaveChangesAsync();

            return new AuthResponse(AuthResult.Success);
        }
        catch (Exception)
        {
            return new AuthResponse(AuthResult.FailedUnknown);
        }
    }
}
