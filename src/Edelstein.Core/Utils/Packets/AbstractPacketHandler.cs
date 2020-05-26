using System.Threading.Tasks;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Utils.Packets
{
    public abstract class AbstractPacketHandler<TAdapter> : IPacketHandler
        where TAdapter : class
    {
        public Task Handle(IPacketHandlerContext ctx)
            => Handle(ctx.Adapter as TAdapter, ctx.Operation, ctx.Packet);

        protected abstract Task Handle(TAdapter adapter, RecvPacketOperations operation, IPacketDecoder packet);
    }
}