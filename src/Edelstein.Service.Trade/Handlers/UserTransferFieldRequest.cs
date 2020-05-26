using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Migrations.States;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Trade.Handlers
{
    public class UserTransferFieldRequest : AbstractPacketHandler<TradeServiceAdapter>
    {
        protected override async Task Handle(
            TradeServiceAdapter adapter,
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