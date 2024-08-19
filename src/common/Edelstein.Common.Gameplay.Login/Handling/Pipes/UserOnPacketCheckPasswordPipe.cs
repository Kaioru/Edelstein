using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;
using Edelstein.Protocol.Network.Packets.Types;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Services.Session.Entities;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketCheckPasswordPipe(
    IAuthService auth,
    ISessionService session,
    IAccountRepository accounts
) : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword> message)
    {
        // TODO state check (logged_out)
        
        try
        {
            var response = await auth.Login(new AuthServiceRequest
            {
                Username = message.Packet.Username.Value,
                Password = message.Packet.Password.Value
            });
            var result = response.Result switch
            {
                AuthServiceResult.Success => LoginResultCode.Success,
                AuthServiceResult.FailedInvalidUsername => LoginResultCode.NotRegistered,
                AuthServiceResult.FailedInvalidPassword => LoginResultCode.IncorrectPassword,
                _ => LoginResultCode.Unknown
            };
            var account = await accounts.RetrieveByUsername(message.Packet.Username.Value) ??
                          await accounts.Insert(new Account
                          {
                              Username = message.Packet.Username.Value
                          });

            if (result == LoginResultCode.Success)
            {
                var sessionResponse = await session.Start(new SessionServiceStartRequest
                {
                    Info = new SessionServiceSessionInfo
                    {
                        ServerID = message.User.System.ID,
                        ActiveAccount = account.ID
                    }
                });

                if (sessionResponse.Result == SessionServiceResult.Success)
                {
                    message.User.Account = account;
                    message.User.Key = sessionResponse.Secret ?? 0;
                }
                else 
                    result = LoginResultCode.AlreadyConnected;
            }

            await message.User.Dispatch(new CheckPasswordResult
            {
                Result = result,
                AccountInfo = result == LoginResultCode.Success
                    ? new AccountInfo
                    {
                        ID = account.ID,
                        NexonClubID = new LPString(account.Username)
                    }
                    : null
            });
        }
        catch
        {
            await message.User.Dispatch(new CheckPasswordResult
            {
                Result = LoginResultCode.Unknown
            });
        }
    }
}
