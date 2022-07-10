using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations
{
    public class BasicConversation : AbstractConversation
    {
        private readonly Action<IConversationSpeaker, IConversationSpeaker> _action;

        public BasicConversation(
            IConversationContext context,
            IConversationSpeaker self,
            IConversationSpeaker target,
            Action<IConversationSpeaker, IConversationSpeaker> action
        ) : base(context, self, target)
        {
            _action = action;
        }

        public override Task Start()
        {
            _action.Invoke(Self, Target);
            return Task.CompletedTask;
        }
    }
}
