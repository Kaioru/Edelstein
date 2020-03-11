using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Service.Game.Conversations.Requests;

namespace Edelstein.Service.Game.Conversations.Speakers
{
    public class DefaultSpeaker : IConversationSpeaker
    {
        public IConversationContext Context { get; }
        public virtual int TemplateID { get; } = 9010000;
        public virtual ConversationSpeakerType Type { get; } = 0;

        public DefaultSpeaker(
            IConversationContext context,
            int templateID = 9010000,
            ConversationSpeakerType param = 0
        )
        {
            Context = context;
            TemplateID = templateID;
            Type = param;
        }

        public IConversationSpeaker AsSpeaker(int templateID, ConversationSpeakerType type = 0)
            => new DefaultSpeaker(Context, templateID, type);

        public IConversationSpeech GetSpeech(string text)
            => new DefaultSpeech(this, text);

        public byte Say(IConversationSpeech[] text, int current = 0)
            => SayAsync(text, current).Result;

        public byte Say(string text = "", bool prev = false, bool next = true)
            => SayAsync(text, prev, next).Result;

        public bool AskYesNo(string text = "")
            => AskYesNoAsync(text).Result;

        public bool AskAccept(string text = "")
            => AskAcceptAsync(text).Result;

        public string AskText(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue)
            => AskTextAsync(text, def, lenMin, lenMax).Result;

        public string AskBoxText(string text = "", string def = "", short cols = 24, short rows = 4)
            => AskBoxTextAsync(text, def, cols, rows).Result;

        public int AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue)
            => AskNumberAsync(text, def, min, max).Result;

        public int AskMenu(string text, IDictionary<int, string> options)
            => AskMenuAsync(text, options).Result;

        public byte AskAvatar(string text, int[] styles)
            => AskAvatarAsync(text, styles).Result;

        public byte AskMemberShopAvatar(string text, int[] styles)
            => AskMemberShopAvatarAsync(text, styles).Result;

        public int AskSlideMenu(IDictionary<int, string> options, int type = 0, int selected = 0)
            => AskSlideMenuAsync(options, type, selected).Result;

        public async Task<byte> SayAsync(IConversationSpeech[] text, int current = 0)
        {
            byte result = 0;

            while (current >= 0 && current < text.Length)
            {
                result = await text[current].Speaker.SayAsync(text[current].Text, current > 0);

                if (result == 0) current = Math.Max(0, --current);
                if (result == 1)
                {
                    if (current == text.Length)
                        break;
                    current = Math.Min(text.Length, ++current);
                }
            }

            return result;
        }

        public Task<byte> SayAsync(string text = "", bool prev = false, bool next = true)
            => Context.Request(new SayRequest(this, text, prev, next));

        public Task<bool> AskYesNoAsync(string text = "")
            => Context.Request(new AskYesNoRequest(this, text));

        public Task<bool> AskAcceptAsync(string text = "")
            => Context.Request(new AskAcceptRequest(this, text));

        public Task<string> AskTextAsync(
            string text = "", string def = "",
            short lenMin = 0, short lenMax = short.MaxValue)
            => Context.Request(new AskTextRequest(this, text, def, lenMin, lenMax));

        public Task<string> AskBoxTextAsync(
            string text = "", string def = "",
            short cols = 24, short rows = 4)
            => Context.Request(new AskBoxTextRequest(this, text, def, cols, rows));

        public Task<int> AskNumberAsync(
            string text = "", int def = 0,
            int min = int.MinValue, int max = int.MaxValue)
            => Context.Request(new AskNumberRequest(this, text, def, min, max));

        public Task<int> AskMenuAsync(string text, IDictionary<int, string> options)
            => Context.Request(new AskMenuRequest(this, text, options));

        public Task<byte> AskAvatarAsync(string text, int[] styles)
            => Context.Request(new AskAvatarRequest(this, text, styles));

        public Task<byte> AskMemberShopAvatarAsync(string text, int[] styles)
            => Context.Request(new AskMemberShopAvatarRequest(this, text, styles));

        public Task<int> AskSlideMenuAsync(IDictionary<int, string> options, int type = 0, int selected = 0)
            => Context.Request(new AskSlideMenuRequest(this, type, selected, options));
    }
}