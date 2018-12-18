using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Entities;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Types;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;

namespace Edelstein.Service.Login.Sockets
{
    public partial class WvsLoginSocket
    {
        private async Task OnCheckPassword(IPacket packet)
        {
            var password = packet.Decode<string>();
            var username = packet.Decode<string>();

            await WvsLogin.LockProvider.AcquireAsync("loginLock");

            using (var p = new Packet(SendPacketOperations.CheckPasswordResult))
            using (var db = WvsLogin.DataContextFactory.Create())
            {
                var result = LoginResult.Success;
                var account = db.Accounts
                    .Include(a => a.Data)
                    .ThenInclude(a => a.Characters)
                    .ThenInclude(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .SingleOrDefault(a => a.Username.Equals(username));
                if (account == null)
                    result = LoginResult.NotRegistered;
                else
                {
                    if (await WvsLogin.AccountStatusCache.ExistsAsync(account.ID.ToString()))
                        result = LoginResult.AlreadyConnected;
                    if (!BCrypt.Net.BCrypt.Verify(password, account.Password))
                        result = LoginResult.IncorrectPassword;
                }

                p.Encode<byte>((byte) result);
                p.Encode<byte>(0);
                p.Encode<int>(0);

                if (result == LoginResult.Success)
                {
                    Account = account;

                    await WvsLogin.AccountStatusCache.SetAsync(
                        account.ID.ToString(),
                        AccountState.LoggingIn
                    );

                    p.Encode<int>(account.ID); // pBlockReason
                    p.Encode<byte>(0); // pBlockReasonIter
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

            await WvsLogin.LockProvider.ReleaseAsync("loginLock");
        }

        private async Task OnWorldInfoRequest(IPacket packet)
        {
            await Task.WhenAll(WvsLogin.Info.Worlds.Select(w =>
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

                    var services = WvsLogin.Peers
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

                    p.Encode<short>((short) WvsLogin.Info.Balloons.Count);
                    WvsLogin.Info.Balloons.ForEach(b =>
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
                p.Encode<int>(WvsLogin.Info.Worlds.FirstOrDefault()?.ID ?? 0);
                await SendPacket(p);
            }
        }

        private async Task OnSelectWorld(IPacket packet)
        {
            packet.Decode<byte>();

            var worldID = packet.Decode<byte>();
            var channelID = packet.Decode<byte>();

            using (var p = new Packet(SendPacketOperations.SelectWorldResult))
            {
                byte result = 0x0;
                var service = WvsLogin.Peers
                    .OfType<GameServiceInfo>()
                    .SingleOrDefault(g => g.ID == channelID &&
                                          g.WorldID == worldID);
                if (service == null) result = 0x1;

                p.Encode<byte>(result);

                if (result == 0)
                {
                    SelectedService = service;

                    using (var db = WvsLogin.DataContextFactory.Create())
                    {
                        var data = Account.Data.SingleOrDefault(d => d.WorldID == worldID);

                        if (data == null)
                        {
                            data = new AccountData
                            {
                                WorldID = worldID,
                                SlotCount = 3,
                                Characters = new List<Character>()
                            };

                            Account.Data.Add(data);
                            db.Update(Account);
                            db.SaveChanges();
                        }

                        var characters = data.Characters;

                        p.Encode<byte>((byte) characters.Count);
                        characters.ForEach(c =>
                        {
                            c.EncodeStats(p);
                            c.EncodeLook(p);

                            p.Encode<bool>(false);
                            p.Encode<bool>(false);
                        });

                        p.Encode<bool>(
                            !string.IsNullOrEmpty(Account.SecondPassword)
                        ); // bLoginOpt TODO: proper bLoginOpt stuff
                        p.Encode<int>(data.SlotCount); // nSlotCount
                        p.Encode<int>(0); // nBuyCharCount
                    }
                }

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
    }
}