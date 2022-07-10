using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages
{
    public class SystemMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.SystemMessage;
        public string Text { get; }

        public SystemMessage(string text)
        {
            Text = text;
        }

        public override void WriteData(IPacketWriter writer)
        {
            writer.WriteString(Text);
        }
    }
}
