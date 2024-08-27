using System;
using System.Data.Common;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Auth.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Auth;

public partial class AuthService
{
    public async Task<AuthServiceResponse> Login(AuthServiceRequest request, CallContext context = default)
    {
        try
        {
            var identity = await repository.RetrieveByUsername(request.Username);
            
            if (identity == null)
                return new AuthServiceResponse
                {
                    Result = AuthServiceResult.FailedInvalidUsername
                };
            Console.WriteLine(BCrypt.Net.BCrypt.EnhancedVerify(request.Password, identity.Password));
            Console.WriteLine(request.Password);
            Console.WriteLine(identity.Password);
            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, identity.Password))
                return new AuthServiceResponse
                {
                    Result = AuthServiceResult.FailedInvalidPassword
                };

            return new AuthServiceResponse
            {
                Result = AuthServiceResult.Success
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
