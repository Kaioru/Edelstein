using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class AskMenuConversationRequest : AbstractConversationRequest<int>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskMenu;
        public string Text { get; }
        public IDictionary<int, string> Menu { get; }

        public AskMenuConversationRequest(
            IConversationSpeaker speaker,
            string text,
            IDictionary<int, string> menu
        ) : base(speaker)
        {
            Text = text;
            Menu = menu;
        }

        public override Task<bool> Check(int response)
            => Task.FromResult(Menu.ContainsKey(response));

        public override void WriteData(IPacketWriter writer)
        {
            writer.WriteString(Text + "\r\n#b" + string.Join(
                "\r\n",
                Menu.Select(p => "#L" + p.Key + "#" + p.Value + "#l")
            ));
        }
    }
}
