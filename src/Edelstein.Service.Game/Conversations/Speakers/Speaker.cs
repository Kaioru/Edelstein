using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Service.Game.Conversations.Messages;
using Edelstein.Service.Game.Conversations.Speakers.Fields;
using Edelstein.Service.Game.Conversations.Speakers.Fields.Continents;

namespace Edelstein.Service.Game.Conversations.Speakers
{
    public class Speaker : ISpeaker
    {
        public IConversationContext Context { get; }
        public virtual int TemplateID { get; } = 9010000;
        public virtual SpeakerParamType ParamType { get; } = 0;

        public Speaker(
            IConversationContext context,
            int templateID = 9010000,
            SpeakerParamType param = 0
        )
        {
            Context = context;
            TemplateID = templateID;
            ParamType = param;
        }

        public FieldSpeaker GetField(int id)
            => new FieldSpeaker(Context, Context.Socket.Service.FieldManager.Get(id));

        public ContinentSpeaker GetContinent(int id)
            => GetField(id).GetContinent();

        public ISpeaker AsSpeaker(int templateID, SpeakerParamType param = 0)
            => new Speaker(Context, templateID, param);

        public ISpeech GetSpeech(string text)
            => new Speech(this, text);

        public byte Say(string[] text, int current = 0)
            => SayAsync(text, current).Result;

        public byte Say(ISpeech[] text, int current = 0)
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

        public Task<byte> SayAsync(string[] text, int current = 0)
            => SayAsync(text
                    .Select(t => (ISpeech) new Speech(this, t))
                    .ToArray(),
                current
            );

        public async Task<byte> SayAsync(ISpeech[] text, int current = 0)
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
            => Context.Request(new SayMessage(this, text, prev, next));

        public Task<bool> AskYesNoAsync(string text = "")
            => Context.Request(new AskYesNoMessage(this, text));

        public Task<bool> AskAcceptAsync(string text = "")
            => Context.Request(new AskAcceptMessage(this, text));

        public Task<string> AskTextAsync(
            string text = "", string def = "",
            short lenMin = 0, short lenMax = short.MaxValue)
            => Context.Request(new AskTextMessage(this, text, def, lenMin, lenMax));

        public Task<string> AskBoxTextAsync(
            string text = "", string def = "",
            short cols = 24, short rows = 4)
            => Context.Request(new AskBoxTextMessage(this, text, def, cols, rows));

        public Task<int> AskNumberAsync(
            string text = "", int def = 0,
            int min = int.MinValue, int max = int.MaxValue)
            => Context.Request(new AskNumberMessage(this, text, def, min, max));

        public Task<int> AskMenuAsync(string text, IDictionary<int, string> options)
            => Context.Request(new AskMenuMessage(this, text, options));

        public Task<byte> AskAvatarAsync(string text, int[] styles)
            => Context.Request(new AskAvatarMessage(this, text, styles));

        public Task<byte> AskMemberShopAvatarAsync(string text, int[] styles)
            => Context.Request(new AskMemberShopAvatarMessage(this, text, styles));

        public Task<int> AskSlideMenuAsync(IDictionary<int, string> options, int type = 0, int selected = 0)
            => Context.Request(new AskSlideMenuMessage(this, type, selected, options));
    }
}