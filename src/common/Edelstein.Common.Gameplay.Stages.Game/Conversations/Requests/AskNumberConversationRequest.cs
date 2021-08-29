using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class AskNumberConversationRequest : AbstractConversationRequest<int>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskNumber;
        public string Text { get; }
        public int Default { get; }
        public int MinNumber { get; }
        public int MaxNumber { get; }

        public AskNumberConversationRequest(
            IConversationSpeaker speaker,
            string text,
            int @default,
            int minNumber,
            int maxNumber
        ) : base(speaker)
        {
            Text = text;
            Default = @default;
            MinNumber = minNumber;
            MaxNumber = maxNumber;
        }

        public override Task<bool> Check(int response)
            => Task.FromResult(response >= MinNumber && response <= MaxNumber);

        public override void WriteData(IPacketWriter writer)
        {
            writer.WriteString(Text);
            writer.WriteInt(Default);
            writer.WriteInt(MinNumber);
            writer.WriteInt(MaxNumber);
        }
    }
}
