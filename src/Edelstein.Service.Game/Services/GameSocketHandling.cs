using System.Linq;
using System.Threading.Tasks;
using Edelstein.Database.Entities;
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
                }
            }
            catch
            {
                await Close();
            }
        }
    }
}