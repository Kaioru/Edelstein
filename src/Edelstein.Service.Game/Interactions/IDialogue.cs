using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Interactions
{
    public interface IDialogue
    {
        Task OnPacket(RecvPacketOperations operation, IPacket packet);
        IPacket GetStartDialoguePacket();
    }
}