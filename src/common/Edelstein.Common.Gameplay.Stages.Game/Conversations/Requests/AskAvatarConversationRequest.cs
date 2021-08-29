using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class AskAvatarConversationRequest : AbstractConversationRequest<byte>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskAvatar;
        public string Text { get; }
        public int[] Styles { get; }

        public AskAvatarConversationRequest(
            IConversationSpeaker speaker,
            string text,
            int[] styles
        ) : base(speaker)
        {
            Text = text;
            Styles = styles;
        }

        public override Task<bool> Check(byte response)
            => Task.FromResult(response >= 0 && response < Styles.Length);

        public override void WriteData(IPacketWriter writer)
        {
            writer.WriteString(Text);
            writer.WriteByte((byte)Styles.Length);
            Styles.ForEach(s => writer.WriteInt(s));
        }
    }
}
