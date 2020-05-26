using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Migrations.States;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Shop.Handlers
{
    public class UserTransferFieldRequest : AbstractPacketHandler<ShopServiceAdapter>
    {
        protected override async Task Handle(
            ShopServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            try
            {
                var service = (await adapter.Service.GetPeers())
                    .Select(p => p.State)
                    .OfType<GameNodeState>()
                    .First(s => s.Name == adapter.LastConnectedService);

                await adapter.TryMigrateTo(service);
            }
            catch
            {
                await adapter.Close();
            }
        }
    }
}