using System.Linq;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.User;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Service.Game.Sockets
{
    public partial class WvsGameSocket
    {
        private async Task OnMigrateIn(IPacket packet)
        {
            var characterID = packet.Decode<int>();

            if (!await WvsGame.TryMigrateFrom(characterID))
                await Disconnect();

            using (var db = WvsGame.DataContextFactory.Create())
            {
                var character = db.Characters
                    .Include(c => c.Data)
                    .ThenInclude(a => a.Account)
                    .Include(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .Single(c => c.ID == characterID);
                var field = WvsGame.FieldManager.Get(character.FieldID);
                var fieldUser = new FieldUser(this, character);

                FieldUser = fieldUser;
                await field.Enter(fieldUser);
            }
        }
    }
}