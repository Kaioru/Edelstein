using System;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class AskBoxTextConversationRequest : AbstractConversationRequest<string>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskBoxText;
        public string Text { get; }
        public string Default { get; }
        public short Rows { get; }
        public short Cols { get; }

        public AskBoxTextConversationRequest(
            IConversationSpeaker speaker,
            string text,
            string @default,
            short rows,
            short cols
        ) : base(speaker)
        {
            Text = text;
            Default = @default;
            Rows = rows;
            Cols = cols;
        }

        public override void WriteData(IPacketWriter writer)
        {
            writer.WriteString(Text);
            writer.WriteString(Default);
            writer.WriteShort(Cols);
            writer.WriteShort(Rows);
        }
    }
}
