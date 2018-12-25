using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Field.User;

namespace Edelstein.Service.Game.Interactions
{
    public interface IDialogue
    {
        Task OnPacket(FieldUser user, RecvPacketOperations operation, IPacket packet);
        IPacket GetStartDialoguePacket();
    }
}