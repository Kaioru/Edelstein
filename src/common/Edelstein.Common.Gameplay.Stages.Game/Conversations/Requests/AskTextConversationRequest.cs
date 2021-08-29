using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class AskTextConversationRequest : AbstractConversationRequest<string>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskText;
        public string Text { get; }
        public string Default { get; }
        public short MinLength { get; }
        public short MaxLength { get; }

        public AskTextConversationRequest(
            IConversationSpeaker speaker,
            string text,
            string @default,
            short minLength,
            short maxLength
        ) : base(speaker)
        {
            Text = text;
            Default = @default;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public override Task<bool> Check(string response)
            => Task.FromResult(response.Length >= MinLength && response.Length <= MaxLength);

        public override void WriteData(IPacketWriter writer)
        {
            writer.WriteString(Text);
            writer.WriteString(Default);
            writer.WriteShort(MinLength);
            writer.WriteShort(MaxLength);
        }
    }
}
