using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Network.Packets;
using Edelstein.Common.Services.Auth.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class CheckPasswordPlug : IPipelinePlug<ICheckPassword>
{
    private readonly IAuthService _auth;
    private readonly ILogger _logger;
    private readonly IAccountRepository _repository;
    private readonly ILoginStage _stage;

    public CheckPasswordPlug(
        ILogger<CheckPasswordPlug> logger,
        IAuthService auth,
        IAccountRepository repository,
        ILoginStage stage
    )
    {
        _logger = logger;
        _auth = auth;
        _repository = repository;
        _stage = stage;
    }

    public async Task Handle(IPipelineContext ctx, ICheckPassword message)
    {
        try
        {
            var response = await _auth.Login(new AuthLoginRequest(message.Username, message.Password));
            var result = response.Result switch
            {
                AuthLoginResult.Success => LoginResult.Success,
                AuthLoginResult.FailedInvalidUsername => LoginResult.NotRegistered,
                AuthLoginResult.FailedInvalidPassword => LoginResult.IncorrectPassword,
                _ => LoginResult.Unknown
            };

            if (result == LoginResult.NotRegistered)
            {
                // TODO: Move autoregister this to plugin
                result = LoginResult.Success;
                await _auth.Register(new AuthRegisterRequest(message.Username, message.Password));
                _logger.LogInformation(
                    "Created new user {Username} with password {Password}",
                    message.Username, message.Password
                );
            }

            var account = await _repository.RetrieveByUsername(message.Username) ??
                          await _repository.Insert(new Account { Username = message.Username });
            var packet = new PacketOut(PacketSendOperations.CheckPasswordResult);

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

                packet.WriteLong(0); // UserKey

                message.User.Account = account;
                message.User.State = LoginState.SelectWorld;

                await _stage.Enter(message.User);
            }

            await message.User.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketOut(PacketSendOperations.CheckPasswordResult);

            packet.WriteByte((byte)LoginResult.DBFail);
            packet.WriteByte(0);
            packet.WriteInt(0);

            await message.User.Dispatch(packet);
        }
    }
}
