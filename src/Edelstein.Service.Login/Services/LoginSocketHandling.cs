using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Inventories;
using Edelstein.Database;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Etc.MakeCharInfo;
using Edelstein.Provider.Templates.Item;
using Edelstein.Service.Login.Logging;
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
                    var account = store
                        .Query<Account>()
                        .FirstOrDefault(a => a.Username == username);
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
                        await TryProcessHeartbeat(Account, Character, true);

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

        private async Task OnSelectWorld(IPacket packet)
        {
            packet.Decode<byte>();

            var worldID = packet.Decode<byte>();
            var channelID = packet.Decode<byte>();

            try
            {
                using (var p = new Packet(SendPacketOperations.SelectWorldResult))
                using (var store = Service.DocumentStore.OpenSession())
                {
                    var service = Service.Peers
                        .OfType<GameServiceInfo>()
                        .FirstOrDefault(g => g.ID == channelID &&
                                             g.WorldID == worldID);
                    var result = LoginResultCode.Success;

                    if (service == null) result = LoginResultCode.NotConnectableWorld;

                    p.Encode<byte>((byte) result);

                    if (result == LoginResultCode.Success)
                    {
                        var data = store
                            .Query<AccountData>()
                            .Where(d => d.AccountID == Account.ID)
                            .FirstOrDefault(d => d.WorldID == worldID);

                        if (data == null)
                        {
                            data = new AccountData
                            {
                                AccountID = Account.ID,
                                WorldID = worldID,
                                SlotCount = 3
                            };

                            store.Store(data);
                            store.SaveChanges();
                        }

                        AccountData = data;
                        SelectedService = service;

                        if (Account.LatestConnectedWorld != worldID)
                        {
                            Account.LatestConnectedWorld = worldID;
                            store.Update(Account);
                            store.SaveChanges();
                        }

                        var characters = store
                            .Query<Character>()
                            .Where(c => c.AccountDataID == data.ID)
                            .ToList();

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

                    await SendPacket(p);
                }
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.SelectWorldResult))
                {
                    p.Encode<byte>((byte) LoginResultCode.Unknown);
                    await SendPacket(p);
                }
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

        private async Task OnCheckDuplicatedID(IPacket packet)
        {
            var name = packet.Decode<string>();

            try
            {
                await Service.LockProvider.AcquireAsync("creationLock");

                using (var p = new Packet(SendPacketOperations.CheckDuplicatedIDResult))
                using (var store = Service.DocumentStore.OpenSession())
                {
                    var duplicatedID = store
                        .Query<Character>()
                        .Any(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                    p.Encode<string>(name);
                    p.Encode<bool>(duplicatedID);
                    await SendPacket(p);
                }
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.CheckDuplicatedIDResult))
                {
                    p.Encode<string>(name);
                    p.Encode<byte>(0x2);
                    await SendPacket(p);
                }
            }
            finally
            {
                await Service.LockProvider.ReleaseAsync("creationLock");
            }
        }

        private async Task OnCreateNewCharacter(IPacket packet)
        {
            var name = packet.Decode<string>();
            var race = (Race) packet.Decode<int>();
            var subJob = packet.Decode<short>();
            var face = packet.Decode<int>();
            var hair = packet.Decode<int>();
            var hairColor = packet.Decode<int>();
            var skin = packet.Decode<int>();
            var coat = packet.Decode<int>();
            var pants = packet.Decode<int>();
            var shoes = packet.Decode<int>();
            var weapon = packet.Decode<int>();
            var gender = packet.Decode<byte>();

            try
            {
                await Service.LockProvider.AcquireAsync("creationLock");

                using (var p = new Packet(SendPacketOperations.CreateNewCharacterResult))
                using (var store = Service.DocumentStore.OpenSession())
                {
                    var character = new Character
                    {
                        AccountDataID = AccountData.ID,
                        Name = name,
                        Job = 0,
                        Face = face,
                        Hair = hair + hairColor,
                        Skin = (byte) skin,
                        Gender = gender,
                        FieldID = 310000000,
                        FieldPortal = 0,
                        Level = 1,
                        HP = 50,
                        MaxHP = 50,
                        MP = 50,
                        MaxMP = 50,
                        SubJob = (short) (race == Race.Normal ? subJob : 0)
                    };
                    var result = LoginResultCode.Success;
                    var templates = Service.TemplateManager;
                    var makeCharInfo = templates
                        .GetAll<MakeCharInfoTemplate>()
                        .FirstOrDefault(
                            i => i.Type == (race switch {
                                     Race.Normal => MakeCharInfoType.Normal,
                                     Race.Cygnus => MakeCharInfoType.Premium,
                                     Race.Aran => MakeCharInfoType.Orient,
                                     Race.Evan => MakeCharInfoType.Evan,
                                     Race.Resistance => MakeCharInfoType.Resistance
                                     }
                                 ) &&
                                 i.Gender == gender);

                    if (makeCharInfo == null) result = LoginResultCode.Unknown;
                    else if (makeCharInfo.Face.All(i => i != face) ||
                             makeCharInfo.Hair.All(i => i != hair) ||
                             makeCharInfo.HairColor.All(i => i != hairColor) ||
                             makeCharInfo.Skin.All(i => i != skin) ||
                             makeCharInfo.Coat.All(i => i != coat) ||
                             makeCharInfo.Pants.All(i => i != pants) ||
                             makeCharInfo.Shoes.All(i => i != shoes) ||
                             makeCharInfo.Weapon.All(i => i != weapon)
                    ) result = LoginResultCode.Unknown;

                    p.Encode<byte>((byte) result);

                    if (result == LoginResultCode.Success)
                    {
                        var context = new ModifyInventoriesContext(character.Inventories);

                        context.Set(-5, templates.Get<ItemTemplate>(coat));
                        context.Set(-7, templates.Get<ItemTemplate>(shoes));
                        context.Set(-11, templates.Get<ItemTemplate>(weapon));
                        if (pants > 0)
                            context.Set(-6, templates.Get<ItemTemplate>(pants));

                        store.Store(character);
                        store.SaveChanges();

                        Logger.Debug($"Created new {race} character, {name}");

                        character.EncodeStats(p);
                        character.EncodeLook(p);
                        p.Encode<bool>(false);
                        p.Encode<bool>(false);
                    }
                    else
                    {
                        p.Encode<int>(0);
                    }

                    await SendPacket(p);
                }
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.CreateNewCharacterResult))
                {
                    p.Encode<byte>((byte) LoginResultCode.DBFail);
                    p.Encode<int>(0);
                    await SendPacket(p);
                }
            }
            finally
            {
                await Service.LockProvider.ReleaseAsync("creationLock");
            }
        }

        private async Task OnEnableSPWRequest(IPacket packet, bool vac)
        {
            packet.Decode<bool>(); // ?
            var characterID = packet.Decode<int>();

            if (vac) packet.Decode<int>(); // Unknown

            packet.Decode<string>(); // sMacAddress
            packet.Decode<string>(); // sMacAddressWithHDDSerial
            var spw = packet.Decode<string>();

            try
            {
                using (var store = Service.DocumentStore.OpenSession())
                {
                    var character = store.Query<Character>()
                        .FirstOrDefault(c => c.ID == characterID);
                    var result = LoginResultCode.Success;

                    if (vac)
                    {
                        AccountData = store.Query<AccountData>()
                            .First(a => a.ID == Character.ID);
                        SelectedService = Service.Peers
                            .OfType<GameServiceInfo>()
                            .First(g => g.WorldID == AccountData.WorldID);

                        if (AccountData.AccountID != Account.ID)
                            result = LoginResultCode.Unknown;
                    }

                    Character = character;

                    if (!string.IsNullOrEmpty(Account.SecondPassword))
                        result = LoginResultCode.Unknown;
                    if (BCrypt.Net.BCrypt.Verify(spw, Account.Password))
                        result = LoginResultCode.SamePasswordAndSPW;

                    if (result != LoginResultCode.Success)
                    {
                        using (var p = new Packet(SendPacketOperations.EnableSPWResult))
                        {
                            p.Encode<bool>(false);
                            p.Encode<byte>((byte) result);
                            await SendPacket(p);
                        }

                        return;
                    }

                    Account.SecondPassword = BCrypt.Net.BCrypt.HashPassword(spw);
                    store.Update(Account);
                    store.SaveChanges();

                    await TryMigrateTo(Account, Character, SelectedService);
                }
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.EnableSPWResult))
                {
                    p.Encode<bool>(false);
                    p.Encode<byte>((byte) LoginResultCode.Unknown);
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

            try
            {
                using (var store = Service.DocumentStore.OpenSession())
                {
                    var character = store.Query<Character>()
                        .FirstOrDefault(c => c.ID == characterID);
                    var result = LoginResultCode.Success;

                    if (vac)
                    {
                        AccountData = store.Query<AccountData>()
                            .First(a => a.ID == Character.ID);
                        SelectedService = Service.Peers
                            .OfType<GameServiceInfo>()
                            .First(g => g.WorldID == AccountData.WorldID);

                        if (AccountData.AccountID != Account.ID)
                            result = LoginResultCode.Unknown;
                    }

                    Character = character;

                    if (string.IsNullOrEmpty(Account.SecondPassword))
                        result = LoginResultCode.Unknown;
                    else if (!BCrypt.Net.BCrypt.Verify(spw, Account.SecondPassword))
                        result = LoginResultCode.IncorrectSPW;

                    if (result != LoginResultCode.Success)
                    {
                        using (var p = new Packet(SendPacketOperations.CheckSPWResult))
                        {
                            p.Encode<byte>((byte) result);
                            await SendPacket(p);
                        }

                        return;
                    }

                    await TryMigrateTo(Account, Character, SelectedService);
                }
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.CheckSPWResult))
                {
                    p.Encode<byte>((byte) LoginResultCode.Unknown);
                    await SendPacket(p);
                }
            }
        }
    }
}