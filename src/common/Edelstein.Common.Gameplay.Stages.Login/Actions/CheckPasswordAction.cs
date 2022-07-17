using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Network.Packets;
using Edelstein.Common.Services.Auth.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Auth.Contracts;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Actions;

public class CheckPasswordAction : IPipelineAction<ICheckPassword>
{
    private readonly IAuthService _auth;
    private readonly ILogger _logger;

    public CheckPasswordAction(ILogger<CheckPasswordAction> logger, IAuthService auth)
    {
        _logger = logger;
        _auth = auth;
    }

    public async Task Handle(IPipelineContext ctx, ICheckPassword message)
    {
        var response = await _auth.Login(new AuthLoginRequest(message.Username, message.Password));
        var result = response.Result switch
        {
            AuthLoginResult.Success => LoginResult.Success,
            AuthLoginResult.FailedInvalidUsername => LoginResult.NotRegistered,
            AuthLoginResult.FailedInvalidPassword => LoginResult.IncorrectPassword,
            _ => LoginResult.Unknown
        };
        var packet = new PacketOut(PacketSendOperations.CheckPasswordResult);

        packet.WriteByte((byte)result);
        packet.WriteByte(0);
        packet.WriteInt(0);

        if (result == LoginResult.Success)
        {
            packet.WriteInt(1);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteShort(0);
            packet.WriteByte(0); // nCountryID
            packet.WriteString(message.Username); // sNexonClubID
            packet.WriteByte(0); // nPurchaseEXP
            packet.WriteByte(0); // ChatUnblockReason
            packet.WriteLong(0); // dtChatUnblockDate
            packet.WriteLong(0); // dtRegisterDate
            packet.WriteInt(4); // nNumOfCharacter
            packet.WriteByte(1); // v44
            packet.WriteByte(0); // sMsg

            packet.WriteLong(0);
        }

        await message.User.Dispatch(packet);
    }
}
