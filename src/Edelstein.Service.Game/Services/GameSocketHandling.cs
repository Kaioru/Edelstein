using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Services
{
    public partial class GameSocket
    {
        private async Task OnMigrateIn(IPacket packet)
        {
            var characterID = packet.Decode<int>();

            try
            {
                using (var store = Service.DataStore.OpenSession())
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

                    await TryMigrateFrom(account, character);

                    Account = account;
                    AccountData = data;
                    Character = character;

                    var field = Service.FieldManager.Get(character.FieldID);
                    var fieldUser = new FieldUser(this);

                    FieldUser = fieldUser;
                    await field.Enter(fieldUser);

                    using (var p = new Packet(SendPacketOperations.FuncKeyMappedInit))
                    {
                        p.Encode<bool>(false);

                        for (var i = 0; i < 90; i++)
                        {
                            var key = character.FunctionKeys[i];

                            p.Encode<byte>(key?.Type ?? 0);
                            p.Encode<int>(key?.Action ?? 0);
                        }

                        await SendPacket(p);
                    }
                }
            }
            catch
            {
                await Close();
            }
        }

        private async Task OnFuncKeyMappedModified(IPacket packet)
        {
            var v3 = packet.Decode<int>();

            if (v3 > 0) return;
            var count = packet.Decode<int>();

            for (var i = 0; i < count; i++)
            {
                var key = packet.Decode<int>();

                Character.FunctionKeys[key] = new FunctionKey
                {
                    Type = packet.Decode<byte>(),
                    Action = packet.Decode<int>()
                };
            }
        }
    }
}