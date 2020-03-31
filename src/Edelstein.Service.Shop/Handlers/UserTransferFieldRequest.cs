using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.States;
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
            IPacket packet
        )
        {
            try
            {
                var service = (await adapter.Service.GetPeers())
                    .Select(p => p.State)
                    .OfType<GameServiceState>()
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