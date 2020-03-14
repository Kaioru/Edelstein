using System.Collections.Generic;

namespace Edelstein.Core.Gameplay.Social.Messages
{
    public class GroupMessageEvent
    {
        public GroupMessageType Type { get; }
        public ICollection<int> Targets { get; }
        public string Name { get; }
        public string Text { get; }

        public GroupMessageEvent(
            GroupMessageType type,
            ICollection<int> targets,
            string name,
            string text
        )
        {
            Type = type;
            Targets = targets;
            Name = name;
            Text = text;
        }
    }
}