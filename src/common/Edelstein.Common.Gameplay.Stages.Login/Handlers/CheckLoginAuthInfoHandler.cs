using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Services.Auth.Contracts;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Services.Server.Contracts.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Util.Buffers.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CheckLoginAuthInfoHandler : AbstractLoginPacketHandler
{
    private readonly IAuthService _auth;
    private readonly ILogger _logger;
    private readonly IAccountRepository _repository;
    private readonly ISessionService _session;
    private readonly ILoginStage _stage;

    public CheckLoginAuthInfoHandler(
        ILogger<CheckLoginAuthInfoHandler> logger,
        IAuthService auth,
        ISessionService session,
        IAccountRepository repository,
        ILoginStage stage
    )
    {
        _logger = logger;
        _auth = auth;
        _session = session;
        _repository = repository;
        _stage = stage;
    }

    public override short Operation => (short)PacketRecvOperations.CheckLoginAuthInfo;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.CheckPassword;

    public override async Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        _ = reader.ReadByte();
        var password = reader.ReadString();
        var username = reader.ReadString();

        try
        {
            var response = await _auth.Login(new AuthRequest(username, password));
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
                await _auth.Register(new AuthRequest(username, password));
                _logger.LogInformation(
                    "Created new user {Username} with password {Password}",
                    username, password
                );
            }

            var account = await _repository.RetrieveByUsername(username) ??
                          await _repository.Insert(new Account { Username = username });

            if (result == LoginResult.Success)
            {
                var session = new Session
                {
                    ServerID = user.Context.Options.ID,
                    ActiveAccount = account.ID
                };

                if ((await _session.Start(new SessionStartRequest(session))).Result != SessionResult.Success)
                    result = LoginResult.AlreadyConnected;
            }

            var packet = new PacketWriter();

            packet.WriteShort(0);

            packet.WriteByte((byte)result);
            packet.WriteByte(0);
            packet.WriteInt(0);

            if (result == LoginResult.Success)
            {
                packet.WriteString(account.Username);
                packet.WriteInt(account.ID);
                packet.WriteByte(account.Gender ?? 0);

                packet.WriteByte(0);
                packet.WriteInt(0);
                packet.WriteInt(18); // Age
                packet.WriteBool(true);

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

                packet.WriteByte(0);
                packet.WriteInt(-1);
                packet.WriteByte(0);
                packet.WriteByte(0);

                user.Key = new Random().NextInt64();

                packet.WriteLong(user.Key);

                user.Account = account;
                user.State = LoginState.SelectWorld;

                await _stage.Enter(user);
            }

            await user.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketWriter();

            packet.WriteShort(0);

            packet.WriteByte((byte)LoginResult.DBFail);
            packet.WriteByte(0);
            packet.WriteInt(0);

            await user.Dispatch(packet);
        }
    }
}
