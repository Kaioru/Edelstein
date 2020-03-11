using System;
using System.Threading.Tasks;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Util
{
    public class ActionConversation : AbstractConversation
    {
        private readonly Action<IConversationSpeaker, IConversationSpeaker> _action;

        public ActionConversation(
            IConversationContext context,
            IConversationSpeaker self,
            IConversationSpeaker target,
            Action<IConversationSpeaker, IConversationSpeaker> action
        ) : base(context, self, target)
        {
            _action = action;
        }

        protected override async Task Run()
            => _action.Invoke(Self, Target);
    }
}