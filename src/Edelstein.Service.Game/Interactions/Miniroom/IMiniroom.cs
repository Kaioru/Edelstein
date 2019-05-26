using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public interface IMiniroom
    {
        MiniroomType Type { get; }
        byte MaxUsers { get; }
        IDictionary<byte, FieldUser> Users { get; }

        Task<MiniroomEnterResult> Enter(IMiniroomDialog dialog);
        Task Leave(IMiniroomDialog dialog, MiniroomLeaveResult leaveResult = MiniroomLeaveResult.UserRequest);

        Task BroadcastPacket(IPacket packet);
        Task BroadcastPacket(FieldUser user, IPacket packet);
    }
}