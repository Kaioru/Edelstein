using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Service.Game.Conversations.Messages;

namespace Edelstein.Service.Game.Conversations
{
    public abstract class AbstractSpeaker : ISpeaker
    {
        private readonly IConversationContext _context;

        public abstract byte TypeID { get; }
        public abstract int TemplateID { get; }
        public abstract ScriptMessageParam Param { get; }

        public AbstractSpeaker(IConversationContext context)
            => _context = context;

        public byte Say(string[] text, int current = 0)
        {
            byte result = 0;

            while (current >= 0 && current < text.Length)
            {
                result = Say(text[current], current > 0, current < text.Length - 1);

                if (result == 0) current = Math.Max(0, --current);
                if (result == 1) current = Math.Min(text.Length, ++current);
            }

            return result;
        }

        public byte Say(string text = "", bool prev = false, bool next = false)
            => _context.Send<byte>(new SayMessage(this, text, prev, next)).Result;

        public bool AskYesNo(string text = "")
            => _context.Send<bool>(new AskYesNoMessage(this, text)).Result;

        public bool AskAccept(string text = "")
            => _context.Send<bool>(new AskAcceptMessage(this, text)).Result;

        public string AskText(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue)
            => _context.Send<string>(new AskTextMessage(this, text, def, lenMin, lenMax)).Result;

        public string AskBoxText(string text = "", string def = "", short cols = 24, short rows = 4)
            => _context.Send<string>(new AskBoxTextMessage(this, text, def, cols, rows)).Result;

        public int AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue)
            => _context.Send<int>(new AskNumberMessage(this, text, def, min, max)).Result;

        public int AskMenu(string text, IDictionary<int, string> options)
            => _context.Send<int>(new AskMenuMessage(
                this,
                text + "\r\n#b" + string.Join("\r\n", options.Select(p => "#L" + p.Key + "#" + p.Value + "#l"))
            )).Result;

        public byte AskAvatar(string text, int[] styles)
            => _context.Send<byte>(new AskAvatarMessage(this, text, styles)).Result;

        public byte AskMembershopAvatar(string text, int[] styles)
            => _context.Send<byte>(new AskMembershopAvatarMessage(this, text, styles)).Result;

        public int AskSlideMenu(IDictionary<int, string> options, int type = 0, int selected = 0)
            => _context.Send<int>(new AskMenuMessage(
                this,
                string.Join("\r\n", options.Select(p => "#L" + p.Key + "#" + p.Value + "#l"))
            )).Result;
    }
}