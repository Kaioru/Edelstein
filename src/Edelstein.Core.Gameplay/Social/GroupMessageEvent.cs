using System.Collections.Generic;

namespace Edelstein.Core.Gameplay.Social
{
    public class GroupMessageEvent
    {
        public GroupMessageType Type { get; }
        public ICollection<int> Recipients { get; }
        public string Name { get; }
        public string Text { get; }

        public GroupMessageEvent(
            GroupMessageType type,
            ICollection<int> recipients,
            string name,
            string text
        )
        {
            Type = type;
            Recipients = recipients;
            Name = name;
            Text = text;
        }
    }
}