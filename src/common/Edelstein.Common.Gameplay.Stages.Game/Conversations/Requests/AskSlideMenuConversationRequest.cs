using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class AskSlideMenuConversationRequest : AbstractConversationRequest<int>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskSlideMenu;
        public int SlideMenuType { get; }
        public int Selected { get; }
        public IDictionary<int, string> Menu { get; }

        public AskSlideMenuConversationRequest(
            IConversationSpeaker speaker,
            int slideMenuType,
            int selected,
            IDictionary<int, string> menu
        ) : base(speaker)
        {
            SlideMenuType = slideMenuType;
            Selected = selected;
            Menu = menu;
        }

        public override Task<bool> Check(int response)
            => Task.FromResult(Menu.ContainsKey(response));

        public override void WriteData(IPacketWriter writer)
        {
            writer.WriteInt(SlideMenuType);
            writer.WriteInt(Selected);
            writer.WriteString(string.Join(
                "\r\n",
                Menu.Select(p => "#L" + p.Key + "#" + p.Value + "#l")
            ));
        }
    }
}
