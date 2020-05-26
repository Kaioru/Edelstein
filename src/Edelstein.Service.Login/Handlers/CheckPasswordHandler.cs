using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Logging;
using Edelstein.Service.Login.Types;

namespace Edelstein.Service.Login.Handlers
{
    public class CheckPasswordHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            var password = packet.DecodeString();
            var username = packet.DecodeString();

            if (adapter.Account != null ||
                adapter.AccountWorld != null ||
                adapter.Character != null
            )
            {
                await adapter.Close();
                return;
            }

            var token = new CancellationTokenSource();

            token.CancelAfter(TimeSpan.FromSeconds(5));

            var authLock =
                await adapter.Service.LockProvider.AcquireAsync(
                    LoginService.AuthLockKey,
                    cancellationToken: token.Token
                );

            if (authLock == null)
            {
                using var p = new OutPacket(SendPacketOperations.CheckPasswordResult);

                p.EncodeByte((byte) LoginResultCode.Timeout);
                p.EncodeByte(0);
                p.EncodeInt(0);

                await adapter.SendPacket(p);
                return;
            }

            try
            {
                using var p = new OutPacket(SendPacketOperations.CheckPasswordResult);
                using var store = adapter.Service.DataStore.StartSession();

                var result = LoginResultCode.Success;
                var account = store
                    .Query<Account>()
                    .Where(a => a.Username == username)
                    .FirstOrDefault();

                if (account == null)
                {
                    if (adapter.Service.State.AutoRegister)
                    {
                        account = new Account
                        {
                            Username = username,
                            Password = BCrypt.Net.BCrypt.HashPassword(password)
                        };

                        await store.InsertAsync(account);
                        Logger.Debug($"Created new account, {username}");
                    }
                    else result = LoginResultCode.NotRegistered;
                }
                else
                {
                    if (await adapter.Service.AccountStateCache.ExistsAsync(account.ID.ToString()))
                        result = LoginResultCode.AlreadyConnected;
                    if (!BCrypt.Net.BCrypt.Verify(password, account.Password))
                        result = LoginResultCode.IncorrectPassword;
                }

                p.EncodeByte((byte) result);
                p.EncodeByte(0);
                p.EncodeInt(0);

                if (result == LoginResultCode.Success)
                {
                    p.EncodeInt(account.ID); // pBlockReason
                    p.EncodeByte(account.Gender ?? (byte) 0xA);
                    p.EncodeByte(0); // nGradeCode
                    p.EncodeShort(0); // nSubGradeCode
                    p.EncodeByte(0); // nCountryID
                    p.EncodeString(account.Username); // sNexonClubID
                    p.EncodeByte(0); // nPurchaseEXP
                    p.EncodeByte(0); // ChatUnblockReason
                    p.EncodeLong(0); // dtChatUnblockDate
                    p.EncodeLong(0); // dtRegisterDate
                    p.EncodeInt(4); // nNumOfCharacter
                    p.EncodeByte(1); // v44
                    p.EncodeByte(0); // sMsg

                    p.EncodeLong(adapter.ClientKey);

                    adapter.Account = account;
                    await adapter.TryConnect();
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