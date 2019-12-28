using System.Threading.Tasks;

namespace Edelstein.Core.Utils.Packets
{
    public interface IPacketHandler
    {
        Task Handle(IPacketHandlerContext ctx);
    }
}