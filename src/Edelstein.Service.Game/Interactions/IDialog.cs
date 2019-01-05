using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Interactions
{
    public interface IDialog
    {
        Task<bool> Enter(FieldUser user);
        
        Task OnPacket(RecvPacketOperations operation, FieldUser user, IPacket packet);
    }
}