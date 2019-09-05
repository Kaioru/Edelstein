using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Gameplay.Social.Messages;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Dragon;
using Edelstein.Service.Game.Fields.Objects.User;
using Marten.Util;
using MoreLinq;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class MigrateInHandler : IGameHandler
    {
        public async Task Handle(RecvPacketOperations operation, IPacket packet, GameSocket socket)
        {
            var characterID = packet.Decode<int>();

            try
            {
                using (var store = socket.Service.DataStore.OpenSession())
                {
                    var character = store
                        .Query<Character>()
                        .First(c => c.ID == characterID);
                    var data = store
                        .Query<AccountData>()
                        .First(d => d.ID == character.AccountDataID);
                    var account = store
                        .Query<Account>()
                        .First(a => a.ID == data.AccountID);

                    await socket.TryMigrateFrom(account, character);

                    socket.Account = account;
                    socket.AccountData = data;
                    socket.Character = character;

                    var field = socket.Service.FieldManager.Get(character.FieldID);
                    var fieldUser = new FieldUser(socket);

                    socket.FieldUser = fieldUser;

                    await field.Enter(fieldUser);

                    if (socket.SocialService != null)
                        await socket.Service.SendMessage(socket.SocialService, new SocialUpdateStateMessage
                        {
                            CharacterID = character.ID,
                            State = MigrationState.LoggedIn,
                            Service = socket.Service.Info.Name
                        });

                    using (var p = new Packet(SendPacketOperations.FuncKeyMappedInit))
                    {
                        p.Encode<bool>(false);

                        for (var i = 0; i < 90; i++)
                        {
                            var key = character.FunctionKeys[i];

                            p.Encode<byte>(key?.Type ?? 0);
                            p.Encode<int>(key?.Action ?? 0);
                        }

                        await socket.SendPacket(p);
                    }

                    using (var p = new Packet(SendPacketOperations.QuickslotMappedInit))
                    {
                        p.Encode<bool>(true);

                        for (var i = 0; i < 8; i++)
                            p.Encode<int>(character.QuickSlotKeys[i]);

                        await socket.SendPacket(p);
                    }

                    if (SkillConstants.HasEvanDragon(fieldUser.Character.Job))
                    {
                        var dragon = new FieldDragon(fieldUser);

                        fieldUser.Owned.Add(dragon);
                        await fieldUser.Field.Enter(dragon);
                    }
                }
            }
            catch
            {
                await socket.Close();
            }
        }
    }
}