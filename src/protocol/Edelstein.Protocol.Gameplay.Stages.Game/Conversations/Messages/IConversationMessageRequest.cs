using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;

public interface IConversationMessageRequest<in T> : IConversationMessage, IPacketWritable
{
    bool Check(T response);
}
