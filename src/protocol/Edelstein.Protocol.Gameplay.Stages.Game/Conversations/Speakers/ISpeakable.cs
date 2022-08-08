namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;

public interface ISpeakable
{
    IConversationSpeaker GetSpeaker(IConversationContext ctx);
}
