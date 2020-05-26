using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Network;

namespace Edelstein.Service.Shop
{
    public class ShopServiceAdapter : AbstractMigrationSocketAdapter
    {
        public ShopService Service { get; }

        public ISocialParty Party { get; set; }
        public ISocialGuild Guild { get; set; }

        public ShopServiceAdapter(
            ISocket socket,
            ShopService service
        ) : base(socket, service)
            => Service = service;

        public override async Task OnDisconnect()
        {
            await base.OnDisconnect();
            if (isMigrating) return;
            if (Guild != null)
                await Guild.UpdateNotifyLoginOrLogout(
                    Character.ID,
                    false
                );
            if (Party != null)
                await Party.UpdateUserMigration(
                    Character.ID,
                    -2,
                    -1
                );
        }
    }
}