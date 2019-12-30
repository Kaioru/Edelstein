using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Types;

namespace Edelstein.Service.Login.Handlers
{
    public class CheckPasswordHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var password = packet.Decode<string>();
            var username = packet.Decode<string>();
            var token = new CancellationTokenSource();

            token.CancelAfter(TimeSpan.FromSeconds(5));

            var authLock =
                await adapter.Service.LockProvider.AcquireAsync(
                    LoginService.AuthLockKey,
                    cancellationToken: token.Token
                );

            if (authLock == null)
            {
                using var p = new Packet(SendPacketOperations.CheckPasswordResult);

                p.Encode<byte>((byte) LoginResultCode.Timeout);
                p.Encode<byte>(0);
                p.Encode<int>(0);

                await adapter.SendPacket(p);
                return;
            }

            try
            {
                using var p = new Packet(SendPacketOperations.CheckPasswordResult);
                using var store = adapter.Service.DataStore.StartSession();

                var result = LoginResultCode.Success;
                var account = store
                    .Query<Account>()
                    .Where(a => a.Username == username)
                    .FirstOrDefault();

                if (account == null) result = LoginResultCode.NotRegistered;
                else
                {
                    if (await adapter.Service.AccountStateCache.ExistsAsync(account.ID.ToString()))
                        result = LoginResultCode.AlreadyConnected;
                    if (!BCrypt.Net.BCrypt.Verify(password, account.Password))
                        result = LoginResultCode.IncorrectPassword;
                }

                p.Encode<byte>((byte) result);
                p.Encode<byte>(0);
                p.Encode<int>(0);

                if (result == LoginResultCode.Success)
                {
                    p.Encode<int>(account.ID); // pBlockReason
                    p.Encode<byte>(account.Gender ?? (byte) 0xA);
                    p.Encode<byte>(0); // nGradeCode
                    p.Encode<short>(0); // nSubGradeCode
                    p.Encode<byte>(0); // nCountryID
                    p.Encode<string>(account.Username); // sNexonClubID
                    p.Encode<byte>(0); // nPurchaseEXP
                    p.Encode<byte>(0); // ChatUnblockReason
                    p.Encode<long>(0); // dtChatUnblockDate
                    p.Encode<long>(0); // dtRegisterDate
                    p.Encode<int>(4); // nNumOfCharacter
                    p.Encode<byte>(1); // v44
                    p.Encode<byte>(0); // sMsg

                    p.Encode<long>(0); // dwHighDateTime

                    adapter.Account = account;
                    await adapter.TryRecvHeartbeat(true);
                }

                await adapter.SendPacket(p);
            }
            finally
            {
                await authLock.ReleaseAsync();
            }
        }
    }
}