using Edelstein.Common.Gameplay.Login.Types;
using Edelstein.Common.Gameplay.Models.Accounts;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketCheckPasswordPlug : IPipelinePlug<UserOnPacketCheckPassword>
{
    private readonly IAuthService _auth;
    private readonly ILogger _logger;
    private readonly IAccountRepository _repository;
    private readonly ISessionService _session;
    private readonly ILoginStage _stage;

    public UserOnPacketCheckPasswordPlug(
        ILogger<UserOnPacketCheckPassword> logger,
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

    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckPassword message)
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

            if (message.User.Context.Options.IsAutoRegister && result == LoginResult.NotRegistered)
            {
                // TODO: Move autoregister this to plugin
                result = LoginResult.Success;
                await _auth.Register(new AuthRequest(message.Username, message.Password));
                _logger.LogInformation(
                    "Created new user {Username} with password {Password}",
                    message.Username, message.Password
                );
            }

            var account = await _repository.RetrieveByUsername(message.Username) ??
                          await _repository.Insert(new Account { Username = message.Username });

            if (result == LoginResult.Success)
            {
                var session = new Session(
                    message.User.Context.Options.ID,
                    account.ID
                );

                if ((await _session.Start(new SessionStartRequest(session))).Result != SessionResult.Success)
                    result = LoginResult.AlreadyConnected;
            }

            using var packet = new PacketWriter(PacketSendOperations.CheckPasswordResult);

            packet.WriteByte((byte)result);
            packet.WriteByte(0);
            packet.WriteInt(0);


            if (result == LoginResult.Success)
            {
                packet.WriteInt(account.ID);
                packet.WriteByte(account.Gender ?? 0);
                packet.WriteByte((byte)account.GradeCode);
                packet.WriteShort((short)account.SubGradeCode);
                packet.WriteByte(0); // nCountryID
                packet.WriteString(account.Username); // sNexonClubID
                packet.WriteByte(0); // nPurchaseEXP
                packet.WriteByte(0); // ChatUnblockReason
                packet.WriteLong(0); // dtChatUnblockDate
                packet.WriteLong(0); // dtRegisterDate
                packet.WriteInt(4); // nNumOfCharacter
                packet.WriteByte(1); // v44
                packet.WriteByte(0); // sMsg

                message.User.Key = new Random().NextInt64();

                packet.WriteLong(message.User.Key);

                message.User.Account = account;
                message.User.State = LoginState.SelectWorld;

                await _stage.Enter(message.User);
            }

            await message.User.Dispatch(packet.Build());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            using var packet = new PacketWriter(PacketSendOperations.CheckPasswordResult);

            packet.WriteByte((byte)LoginResult.DBFail);
            packet.WriteByte(0);
            packet.WriteInt(0);

            await message.User.Dispatch(packet.Build());
        }
    }
}
