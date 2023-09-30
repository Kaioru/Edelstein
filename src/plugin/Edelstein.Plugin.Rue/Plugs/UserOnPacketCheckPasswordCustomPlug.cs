using Edelstein.Common.Gameplay.Login.Types;
using Edelstein.Common.Gameplay.Models.Accounts;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Plugin.Rue.Login;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Plugin.Rue.Plugs;

public class UserOnPacketCheckPasswordCustomPlug : IPipelinePlug<UserOnPacketCheckPassword>
{
    private readonly ILoginManager _loginManager;

    public UserOnPacketCheckPasswordCustomPlug(ILoginManager loginManager) => _loginManager = loginManager;

    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckPassword message)
    {
        LoginResult result;

        switch (_loginManager.Options.SkipAuthorization)
        {
            case true: result = LoginResult.Success; break;
            case false:
            {
                var response = await _loginManager.Auth.Login(new AuthRequest(message.Username, message.Password));

                result = response.Result switch
                {
                    AuthResult.Success => LoginResult.Success,
                    AuthResult.FailedInvalidUsername => LoginResult.NotRegistered,
                    AuthResult.FailedInvalidPassword => LoginResult.IncorrectPassword,
                    _ => LoginResult.Unknown
                };
                break;
            }
        }

        var account = await _loginManager.Repository.RetrieveByUsername(message.Username) ??
                      await _loginManager.Repository.Insert(new Account { Username = message.Username });

        if (result == LoginResult.Success)
        {
            var session = new Session(
                message.User.Context.Options.ID,
                account.ID
            );

            if ((await _loginManager.Session.Start(new SessionStartRequest(session))).Result != SessionResult.Success)
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

            await _loginManager.Stage.Enter(message.User);
        }

        await message.User.Dispatch(packet.Build());
        ctx.Cancel();
    }
}
