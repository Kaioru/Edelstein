using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Speakers
{
    public class BasicSpeaker : IConversationSpeaker
    {
        public IConversationContext Context { get; }

        public int TemplateID { get; }
        public ConversationSpeakerFlags Flags { get; }

        public BasicSpeaker(
            IConversationContext context,
            int templateID = 9010000,
            ConversationSpeakerFlags flags = 0
        )
        {
            Context = context;
            TemplateID = templateID;
            Flags = flags;
        }

        public byte Say(string text = "", bool prev = false, bool next = true)
            => Context.Request(new SayConversationRequest(this, text, prev, next)).Result;

        public bool AskYesNo(string text = "")
            => Context.Request(new AskYesNoConversationRequest(this, text)).Result;

        public bool AskAccept(string text = "")
            => Context.Request(new AskAcceptConvesationRequest(this, text)).Result;

        public string AskText(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue)
            => Context.Request(new AskTextConversationRequest(this, text, def, lenMin, lenMax)).Result;

        public string AskBoxText(string text = "", string def = "", short rows = 4, short cols = 24)
            => Context.Request(new AskBoxTextConversationRequest(this, text, def, rows, cols)).Result;

        public int AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue)
            => Context.Request(new AskNumberConversationRequest(this, text, def, min, max)).Result;

        public int AskMenu(string text, IDictionary<int, string> options)
            => Context.Request(new AskMenuConversationRequest(this, text, options)).Result;

        public byte AskAvatar(string text, int[] styles)
            => Context.Request(new AskAvatarConversationRequest(this, text, styles)).Result;

        public byte AskMemberShopAvatar(string text, int[] styles)
            => Context.Request(new AskMemberShopAvatarConversationRequest(this, text, styles)).Result;

        public int AskSlideMenu(IDictionary<int, string> options, int type = 0, int selected = 0)
            => Context.Request(new AskSlideMenuConversationRequest(this, type, selected, options)).Result;
    }
}
