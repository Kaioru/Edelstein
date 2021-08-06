using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Network;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers
{
    public class CheckPasswordHandler : AbstractPacketHandler<LoginStage, LoginStageUser>
    {
        public override short Operation => (short)PacketRecvOperations.CheckPassword;
        private readonly LoginStage _stage;

        public CheckPasswordHandler(LoginStage stage)
            => _stage = stage;

        public override Task<bool> Check(LoginStageUser user)
            => Task.FromResult(user.State == LoginState.LoggedOut);

        public override async Task Handle(LoginStageUser user, IPacketReader packet)
        {
            var password = packet.ReadString();
            var username = packet.ReadString();
            _ = packet.ReadBytes(16); // MachineID
            _ = packet.ReadInt();           // GameRoomClient
            _ = packet.ReadByte();          // m_nGameStartMode
            _ = packet.ReadByte();          // Unknown1
            _ = packet.ReadByte();          // Unknown2
            _ = packet.ReadInt();           // PartnerCode

            var result = LoginResultCode.Success;
            var account = await _stage.AccountRepository.RetrieveByUsername(username);

            if (account == null)
            {
                if (_stage.Config.AutoRegister)
                {
                    account = new Account
                    {
                        Username = username,
                        Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password)
                    };

                    await _stage.AccountRepository.Insert(account);
                    _stage.Logger.LogInformation($"Created new account {account.Username} (ID: {account.ID})");
                }
                else result = LoginResultCode.NotRegistered;
            }
            else
            {
                var session = await _stage.SessionRegistry.DescribeSessionByAccount(new DescribeSessionByAccountRequest { Account = account.ID });

                if (!BCrypt.Net.BCrypt.EnhancedVerify(password, account.Password))
                    result = LoginResultCode.IncorrectPassword;

                if (session.Session.State != SessionState.Offline)
                    result = LoginResultCode.AlreadyConnected;

                if (account.Banned)
                    result = LoginResultCode.Blocked;
            }

            var response = new UnstructuredOutgoingPacket(PacketSendOperations.CheckPasswordResult);

            response.WriteByte((byte)result); // nRet
            response.WriteByte(0);            // nRegStatID
            response.WriteInt(0);             // nUseDay

            switch (result)
            {
                case LoginResultCode.Blocked:
                    response.WriteByte(account.BlockReason);       // nBlockReason
                    response.WriteDateTime(account.DateUnblocked); // dtUnblockDate
                    break;
                case LoginResultCode.Success:
                    response.WriteInt(account.ID);             // dwAccountId
                    response.WriteByte(account.Gender ?? 0xA); // nGender
                    response.WriteByte(0);                     // nGradeCode
                    response.WriteShort(0);                    // nSubGradeCode
                    response.WriteByte(0);                     // nCountryID
                    response.WriteString(account.Username);    // sNexonClubID
                    response.WriteByte(0);                     // nPurchaseEXP
                    response.WriteByte(0);                     // nChatUnblockReason
                    response.WriteLong(0);                     // dtChatUnblockDate
                    response.WriteLong(0);                     // dtRegisterDate
                    response.WriteInt(4);                      // nNumOfCharacter
                    response.WriteByte(1);                     // CP_WorldRequest or nGameStartMode -> CP_CheckPinCode
                    response.WriteByte(0);                     // sMsg

                    response.WriteLong(user.Key);

                    user.State = account.Gender == null ? LoginState.SelectGender : LoginState.SelectWorld;
                    user.Account = account;

                    await _stage.Enter(user);
                    break;
            }

            await user.Dispatch(response);
        }
    }
}
