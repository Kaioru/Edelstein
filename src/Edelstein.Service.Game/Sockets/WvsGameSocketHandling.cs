using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.User;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Service.Game.Sockets
{
    public partial class WvsGameSocket
    {
        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();

            switch (operation)
            {
                case RecvPacketOperations.MigrateIn:
                    return OnMigrateIn(packet);
                default:
                    return FieldUser?.OnPacket(operation, packet);
            }
        }

        private async Task OnMigrateIn(IPacket packet)
        {
            var characterID = packet.Decode<int>();

            using (var db = WvsGame.DataContextFactory.Create())
            {
                var character = db.Characters
                    .Include(c => c.Data)
                    .ThenInclude(a => a.Account)
                    .Include(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .Single(c => c.ID == characterID);

                if (!await WvsGame.TryMigrateFrom(character, WvsGame.Info))
                    await Disconnect();

                var field = WvsGame.FieldManager.Get(character.FieldID);
                var fieldUser = new FieldUser(this, character);

                FieldUser = fieldUser;
                await field.Enter(fieldUser);
            }
        }
    }
}