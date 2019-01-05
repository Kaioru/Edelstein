using System.Threading.Tasks;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public interface IMiniRoom : IDialog
    {
        Task Leave(FieldUser user, MiniRoomLeaveType type = MiniRoomLeaveType.Kicked);
        Task Close(MiniRoomLeaveType type = MiniRoomLeaveType.Closed);
        Task Chat(FieldUser user, string message);

        Task BroadcastPacket(IPacket packet);
        Task BroadcastPacket(FieldUser source, IPacket packet);
        Task OnPacket(MiniRoomAction action, FieldUser user, IPacket packet);
    }
}