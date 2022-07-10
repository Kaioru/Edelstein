using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class AskYesNoConversationRequest : AbstractConversationRequest<bool>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskYesNo;
        public string Text { get; }

        public AskYesNoConversationRequest(IConversationSpeaker speaker, string text) : base(speaker)
        {
            Text = text;
        }

        public override void WriteData(IPacketWriter writer)
        {
            writer.WriteString(Text);
        }
    }
}
