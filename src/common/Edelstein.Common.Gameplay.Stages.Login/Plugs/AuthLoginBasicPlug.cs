using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Services.Auth.Contracts;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Services.Server.Contracts.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class AuthLoginBasicPlug : IPipelinePlug<IAuthLoginBasic>
{
    private ILogger _logger;

    private ILoginStage _stage;

    private IAuthService _auth;
    private ISessionService _session;

    private IAccountRepository _accounts;

    public AuthLoginBasicPlug(
        ILogger<AuthLoginBasicPlug> logger,
        ILoginStage stage,
        IAuthService auth,
        ISessionService session,
        IAccountRepository accounts
    )
    {
        _logger = logger;
        _stage = stage;
        _auth = auth;
        _session = session;
        _accounts = accounts;
    }

    public async Task Handle(IPipelineContext ctx, IAuthLoginBasic message)
    {
        try
        {
            var response = await _auth.Login(new AuthRequest(message.Username, message.Password));
            var result = response.Result switch
            {
                AuthResult.Success => LoginResult.Success,
                AuthResult.FailedInvalidUsername => LoginResult.NotRegistered,
                AuthResult.FailedInvalidPassword => LoginResult.IncorrectPassword,
                _ => LoginResult.Unknown
            };

            if (result == LoginResult.NotRegistered)
            {
                // TODO: Move autoregister this to plugin
                result = LoginResult.Success;
                await _auth.Register(new AuthRequest(message.Username, message.Password));
                _logger.LogInformation(
                    "Created new user {Username} with password {Password}",
                    message.Username, message.Password
                );
            }

            var account = await _accounts.RetrieveByUsername(message.Username) ??
                          await _accounts.Insert(new Account { Username = message.Username });

            if (result == LoginResult.Success)
            {
                var session = new Session
                {
                    ServerID = message.User.Context.Options.ID,
                    ActiveAccount = account.ID
                };

                if ((await _session.Start(new SessionStartRequest(session))).Result != SessionResult.Success)
                    result = LoginResult.AlreadyConnected;
            }

            var packet = new PacketWriter(PacketSendOperations.CheckPasswordResult);

            packet.WriteByte((byte)result);
            packet.WriteByte(0);
            packet.WriteInt(0);

            if (result == LoginResult.Success)
            {
                packet.WriteString(account.Username);
                packet.WriteInt(account.ID);
                packet.WriteByte(account.Gender ?? 0);

                packet.WriteByte(0);
                packet.WriteInt((int)account.GradeCode);
                packet.WriteInt(0); // Age

                packet.WriteBool(true); // CensoredNexonClubID?

                packet.WriteString(account.Username); // sNexonClubID
                packet.WriteByte(0); // nPurchaseEXP
                packet.WriteByte(0); // ChatUnblockReason
                packet.WriteLong(0); // dtChatUnblockDate
                packet.WriteLong(0); // dtRegisterDate
                packet.WriteInt(4); // nNumOfCharacter

                // jobconstants
                packet.WriteBool(true);
                packet.WriteByte(8);

                for (var i = 0; i <= 22; i++)
                {
                    packet.WriteByte(1);
                    packet.WriteShort(1);
                }

                packet.WriteByte((byte)account.SubGradeCode);
                packet.WriteInt(-1);
                packet.WriteByte(0);
                packet.WriteByte(0);

                message.User.Key = new Random().NextInt64();

                packet.WriteLong(message.User.Key);

                message.User.Account = account;
                message.User.State = LoginState.SelectWorld;

                await _stage.Enter(message.User);
            }

            await message.User.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketWriter(PacketSendOperations.CheckPasswordResult);

            packet.WriteByte((byte)LoginResult.DBFail);
            packet.WriteByte(0);
            packet.WriteInt(0);

            await message.User.Dispatch(packet);
        }
    }
}
