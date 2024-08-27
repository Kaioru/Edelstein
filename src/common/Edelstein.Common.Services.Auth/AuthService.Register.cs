using System.Data.Common;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Auth.Contracts;
using EntityFramework.Exceptions.Common;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Auth;

public partial class AuthService
{
    public async Task<AuthServiceResponse> Register(AuthServiceRequest request, CallContext context = default)
    {
        try
        {
            await repository.Insert(new Identity
            {
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password)
            });

            return new AuthServiceResponse
            {
                Result = AuthServiceResult.Success
            };
        }
        catch (UniqueConstraintException)
        {
            return new AuthServiceResponse
            {
                Result = AuthServiceResult.FailedUsernameExists
            };
        } 
        catch (DbException)
        {
            return new AuthServiceResponse
            {
                Result = AuthServiceResult.FailedUnknown
            };
        }
    }
}
