using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Extensions;
using Edelstein.Core.Inventories;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Data;
using Edelstein.Data.Entities;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item;
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
                var result = LoginResult.Success;
                var service = WvsLogin.Peers
                    .OfType<GameServiceInfo>()
                    .SingleOrDefault(g => g.ID == channelID &&
                                          g.WorldID == worldID);
                if (service == null) result = LoginResult.NotConnectableWorld;
                else
                {
                    if (service.AdultChannel) result = LoginResult.NotAdult; // TODO: proper checks
                }

                p.Encode<byte>((byte) result);

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

        private async Task OnCheckDuplicatedID(IPacket packet)
        {
            var name = packet.Decode<string>();

            await WvsLogin.LockProvider.AcquireAsync("characterCreationLock");

            using (var db = WvsLogin.DataContextFactory.Create())
            {
                var isDuplicatedID = db.Characters.Any(c => c.Name.Equals(name));

                using (var p = new Packet(SendPacketOperations.CheckDuplicatedIDResult))
                {
                    p.Encode<string>(name);
                    p.Encode<bool>(isDuplicatedID);

                    await SendPacket(p);
                }
            }

            await WvsLogin.LockProvider.ReleaseAsync("characterCreationLock");
        }

        private async Task OnCreateNewCharacter(IPacket packet)
        {
            var name = packet.Decode<string>();
            var race = packet.Decode<int>();
            var subJob = packet.Decode<short>();
            var face = packet.Decode<int>();
            var hair = packet.Decode<int>() + packet.Decode<int>();
            var skin = packet.Decode<int>();
            var top = packet.Decode<int>();
            var bottom = packet.Decode<int>();
            var shoes = packet.Decode<int>();
            var weapon = packet.Decode<int>();
            var gender = packet.Decode<byte>();

            await WvsLogin.LockProvider.AcquireAsync("characterCreationLock");

            using (var p = new Packet(SendPacketOperations.CreateNewCharacterResult))
            using (var db = WvsLogin.DataContextFactory.Create())
            {
                var result = LoginResult.Success;
                var character = new Character
                {
                    Name = name,
                    Job = 0,
                    Face = face,
                    Hair = hair,
                    Skin = (byte) skin,
                    Gender = gender,
                    FieldID = 310000000,
                    FieldPortal = 0,
                    Level = 1,
                    HP = 50,
                    MaxHP = 50,
                    MP = 50,
                    MaxMP = 50,
                    Inventories = new List<ItemInventory>()
                };


                var inventories = character.Inventories;

                inventories.Add(new ItemInventory(ItemInventoryType.Equip, 24));
                inventories.Add(new ItemInventory(ItemInventoryType.Use, 24));
                inventories.Add(new ItemInventory(ItemInventoryType.Setup, 24));
                inventories.Add(new ItemInventory(ItemInventoryType.Etc, 24));
                inventories.Add(new ItemInventory(ItemInventoryType.Cash, 24));

                var context = new ModifyInventoryContext(character);
                var templates = WvsLogin.TemplateManager;

                context.Set(templates.Get<ItemTemplate>(top), -5);
                if (bottom > 0)
                    context.Set(templates.Get<ItemTemplate>(bottom), -6);
                context.Set(templates.Get<ItemTemplate>(shoes), -7);
                context.Set(templates.Get<ItemTemplate>(weapon), -11);

                var data = Account.Data.FirstOrDefault(a => a.WorldID == SelectedService.WorldID);
                if (data == null) result = LoginResult.DBFail;

                p.Encode<byte>((byte) result);

                if (result == LoginResult.Success)
                {
                    data.Characters.Add(character);
                    db.Update(Account);
                    db.SaveChanges();

                    character.EncodeStats(p);
                    character.EncodeLook(p);
                    p.Encode<bool>(false);
                    p.Encode<bool>(false);
                }

                await SendPacket(p);
            }

            await WvsLogin.LockProvider.ReleaseAsync("characterCreationLock");
        }

        private async Task OnEnableSPWRequest(IPacket packet, bool vac)
        {
            packet.Decode<bool>(); // ?
            var characterID = packet.Decode<int>();

            if (vac) packet.Decode<int>(); // Unknown

            packet.Decode<string>(); // sMacAddress
            packet.Decode<string>(); // sMacAddressWithHDDSerial
            var spw = packet.Decode<string>();

            var result = LoginResult.Success;

            using (var db = WvsLogin.DataContextFactory.Create())
            {
                var character = Account.Data
                    .SelectMany(a => a.Characters)
                    .Single(c => c.ID == characterID);
                SelectedCharacter = character;
                if (!string.IsNullOrEmpty(Account.SecondPassword)) result = LoginResult.Unknown;
                if (BCrypt.Net.BCrypt.Verify(spw, Account.Password)) result = LoginResult.SamePasswordAndSPW;

                if (result == LoginResult.Success)
                {
                    Account.SecondPassword = BCrypt.Net.BCrypt.HashPassword(spw);
                    db.Update(Account);
                    db.SaveChanges();

                    if (vac)
                    {
                        SelectedService = WvsLogin.Peers
                            .OfType<GameServiceInfo>()
                            .First(g => g.WorldID == character.Data.WorldID);
                    }

                    if (!await Migrate(SelectedService)) result = LoginResult.AlreadyConnected;
                }

                if (result == LoginResult.Success) return;
                using (var p = new Packet(SendPacketOperations.EnableSPWResult))
                {
                    p.Encode<bool>(false);
                    p.Encode<byte>((byte) result);
                    await SendPacket(p);
                }
            }
        }

        private async Task OnCheckSPWRequest(IPacket packet, bool vac)
        {
            var spw = packet.Decode<string>();
            var characterID = packet.Decode<int>();
            packet.Decode<string>(); // sMacAddress
            packet.Decode<string>(); // sMacAddressWithHDDSerial

            var result = LoginResult.Success;
            var character = Account.Data
                .SelectMany(a => a.Characters)
                .Single(c => c.ID == characterID);
            SelectedCharacter = character;

            if (string.IsNullOrEmpty(Account.SecondPassword)) result = LoginResult.Unknown;
            else if (!BCrypt.Net.BCrypt.Verify(spw, Account.SecondPassword)) result = LoginResult.IncorrectSPW;

            if (result == LoginResult.Success)
            {
                if (vac)
                {
                    SelectedService = WvsLogin.Peers
                        .OfType<GameServiceInfo>()
                        .First(g => g.WorldID == character.Data.WorldID);
                }

                if (!await Migrate(SelectedService)) result = LoginResult.AlreadyConnected;
            }

            if (result == LoginResult.Success) return;
            using (var p = new Packet(SendPacketOperations.CheckSPWResult))
            {
                p.Encode<byte>((byte) result);
                await SendPacket(p);
            }
        }
    }
}