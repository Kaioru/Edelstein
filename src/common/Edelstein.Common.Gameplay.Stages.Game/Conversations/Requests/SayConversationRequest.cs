using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class SayConversationRequest : AbstractConversationRequest<byte>
    {
        public override ConversationRequestType Type => ConversationRequestType.Say;
        public string Text { get; }
        public bool IsPrevEnabled { get; }
        public bool IsNextEnabled { get; }

        public SayConversationRequest(
            IConversationSpeaker speaker,
            string text,
            bool isPrevEnabled,
            bool isNextEnabled
        ) : base(speaker)
        {
            Text = text;
            IsPrevEnabled = isPrevEnabled;
            IsNextEnabled = isNextEnabled;
        }

        public override void WriteData(IPacketWriter writer)
        {
            if (Speaker.Flags.HasFlag(ConversationSpeakerFlags.NPCReplacedByNPC))
                writer.WriteInt(Speaker.TemplateID);
            writer.WriteString(Text);
            writer.WriteBool(IsPrevEnabled);
            writer.WriteBool(IsNextEnabled);
        }
    }
}
