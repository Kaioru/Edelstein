using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Database;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Types;
using MoreLinq.Extensions;

namespace Edelstein.Service.Login.Services
{
    public partial class LoginSocket
    {
        private async Task OnCheckPassword(IPacket packet)
        {
            var password = packet.Decode<string>();
            var username = packet.Decode<string>();

            try
            {
                await Service.LockProvider.AcquireAsync("loginLock");

                using (var p = new Packet(SendPacketOperations.CheckPasswordResult))
                using (var store = Service.DocumentStore.OpenSession())
                {
                    var account = store.Query<Account>().FirstOrDefault(a => a.Username == username);
                    var result = LoginResultCode.Success;

                    if (account == null) result = LoginResultCode.NotRegistered;
                    else
                    {
                        if (await Service.AccountStateCache.ExistsAsync(account.ID.ToString()))
                            result = LoginResultCode.AlreadyConnected;
                        if (!BCrypt.Net.BCrypt.Verify(password, account.Password))
                            result = LoginResultCode.IncorrectPassword;
                    }

                    p.Encode<byte>((byte) result);
                    p.Encode<byte>(0);
                    p.Encode<int>(0);

                    if (result == LoginResultCode.Success)
                    {
                        Account = account;
                        await Service.AccountStateCache.SetAsync(
                            account.ID.ToString(),
                            MigrationState.LoggedIn
                        );

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
                    }

                    await SendPacket(p);
                }
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.CheckPasswordResult))
                {
                    p.Encode<byte>((byte) LoginResultCode.Unknown);
                    p.Encode<byte>(0);
                    p.Encode<int>(0);

                    await SendPacket(p);
                }
            }
            finally
            {
                await Service.LockProvider.ReleaseAsync("loginLock");
            }
        }

        private async Task OnWorldInfoRequest(IPacket packet)
        {
            await Task.WhenAll(Service.Info.Worlds.Select(w =>
            {
                using (var p = new Packet(SendPacketOperations.WorldInformation))
                {
                    p.Encode<byte>(w.ID);
                    p.Encode<string>(w.Name);
                    p.Encode<byte>(w.State);
                    p.Encode<string>(w.EventDesc);
                    p.Encode<short>(w.EventEXP);
                    p.Encode<short>(w.EventDrop);
                    p.Encode<bool>(w.BlockCharCreation);

                    var services = Service.Peers
                        .OfType<GameServiceInfo>()
                        .Where(g => g.WorldID == w.ID)
                        .OrderBy(g => g.ID)
                        .ToImmutableList();

                    p.Encode<byte>((byte) services.Count);
                    services.ForEach(g =>
                    {
                        p.Encode<string>(g.Name);
                        p.Encode<int>(0); // UserNo
                        p.Encode<byte>(g.WorldID);
                        p.Encode<byte>(g.ID);
                        p.Encode<bool>(g.AdultChannel);
                    });

                    p.Encode<short>((short) Service.Info.Balloons.Count);
                    Service.Info.Balloons.ForEach(b =>
                    {
                        p.Encode<Point>(b.Position);
                        p.Encode<string>(b.Message);
                    });
                    return SendPacket(p);
                }
            }));

            using (var p = new Packet(SendPacketOperations.WorldInformation))
            {
                p.Encode<byte>(0xFF);
                await SendPacket(p);
            }

            using (var p = new Packet(SendPacketOperations.LatestConnectedWorld))
            {
                p.Encode<int>(Account.LatestConnectedWorld);
                await SendPacket(p);
            }
        }
        
        private async Task OnCheckUserLimit(IPacket packet)
        {
            using (var p = new Packet(SendPacketOperations.CheckUserLimitResult))
            {
                p.Encode<byte>(0); // bOverUserLimit
                p.Encode<byte>(0); // bPopulateLevel

                await SendPacket(p);
            }
        }

        private async Task OnSetGender(IPacket packet)
        {
            var gender = packet.Decode<bool>();

            try
            {
                using (var p = new Packet(SendPacketOperations.SetAccountResult))
                using (var store = Service.DocumentStore.OpenSession())
                {
                    Account.Gender = (byte) (gender ? 1 : 0);
                    store.Update(Account);
                    store.SaveChanges();

                    p.Encode<bool>(gender);
                    p.Encode<bool>(true);
                    await SendPacket(p);
                }
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.SetAccountResult))
                {
                    p.Encode<bool>(false);
                    p.Encode<bool>(false);
                    await SendPacket(p);
                }
            }
        }

        private async Task OnCheckPinCode(IPacket packet)
        {
            using (var p = new Packet(SendPacketOperations.CheckPinCodeResult))
            {
                p.Encode<byte>(0);
                await SendPacket(p);
            }
        }
    }
}