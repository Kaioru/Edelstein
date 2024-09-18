using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

public record StructuredScriptMessage() : StructuredSendPacket((short)PacketSendOperation.ScriptMessage)
{
    
}
