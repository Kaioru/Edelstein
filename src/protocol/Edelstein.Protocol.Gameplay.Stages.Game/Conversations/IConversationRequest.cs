using System.Threading.Tasks;
using Edelstein.Protocol.Network.Utils;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations
{
    public interface IConversationRequest<T> : IPacketWritable
    {
        ConversationRequestType Type { get; }

        Task<bool> Check(T response);
    }
}
