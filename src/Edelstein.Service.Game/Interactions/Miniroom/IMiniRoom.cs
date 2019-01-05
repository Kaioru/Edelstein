using System.Threading.Tasks;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public interface IMiniRoom : IDialog
    {
        Task Leave(FieldUser participant, MiniRoomLeaveType type = MiniRoomLeaveType.Kicked);
        Task Close(MiniRoomLeaveType type = MiniRoomLeaveType.DestoryByAdmin);

        Task BroadcastPacket(IPacket packet);
        Task BroadcastPacket(FieldUser source, IPacket packet);
        Task OnPacket(MiniRoomAction action, FieldUser user, IPacket packet);
    }
}