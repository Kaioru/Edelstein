using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

public interface IConversationMessageRequest<in T> : IConversationMessage, IPacketWritable
{
    bool Check(T response);
}
